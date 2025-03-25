using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.DJMaxOnline.Server;

public class PacketFactory
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(PacketFactory));

    private const int PacketHeaderSize = 5;
    private const int PacketIdSize = 2;

    private bool _readPacketId;
    private int _dataSize;
    private int _position;
    private IBuffer _buffer;
    private DjMaxCrypto? _crypto;
    private PacketMeta _packetMeta;

    public PacketFactory()
    {
        _readPacketId = false;
        _dataSize = 0;
        _position = 0;
        _buffer = new StreamBuffer();
        _crypto = null;
        _packetMeta = null;
    }


    public void InitCrypto(DjMaxCrypto crypto)
    {
        _crypto = crypto;
    }

    public byte[] Write(Packet packet)
    {
        byte[] packetData = packet.Data;

        if (_crypto != null)
        {
            Span<byte> packetDataView = packetData;
            _crypto.Encrypt(ref packetDataView);
        }

        IBuffer buffer = new StreamBuffer();
        buffer.WriteUInt16((ushort)packet.Id);
        buffer.WriteBytes(packetData);
        return buffer.GetAllBytes();
    }

    public List<Packet> Read(byte[] data)
    {
        List<Packet> packets = new List<Packet>();
        _buffer.SetPositionEnd();
        _buffer.WriteBytes(data);
        _buffer.Position = _position;

        bool read = true;
        while (read)
        {
            read = false;
            if (!_readPacketId && _buffer.Size - _buffer.Position >= PacketIdSize)
            {
                ushort packetIdNum = _buffer.ReadUInt16();

                if (!Enum.IsDefined(typeof(PacketId), packetIdNum))
                {
                    // TODO err
                    Logger.Error($"packetIdNum: {packetIdNum} is not a defined PacketId");
                }

                PacketId packetId = (PacketId)packetIdNum;

                if (!PacketMeta.TryGet(packetId, out _packetMeta))
                {
                    // TODO err
                    Logger.Error($"PacketMeta not defined for packetId: {packetId}");
                }

                _dataSize = _packetMeta.Size - PacketIdSize;
                _readPacketId = true;
            }

            if (_readPacketId && _buffer.Size - _buffer.Position >= _dataSize)
            {
                byte[]? header = null;
                if (_dataSize >= PacketHeaderSize)
                {
                    // TODO revise some small packets might not have a header (pingTest)
                    // however does it impy all other packets have 5 bytes of header?
                    header = _buffer.ReadBytes(PacketHeaderSize);
                }

                byte[] packetData = _buffer.ReadBytes(_dataSize);
                if (_crypto == null)
                {
                    if (_packetMeta.Id == PacketId.OnConnectAck)
                    {
                        _crypto = DjMaxCrypto.FromOnConnectAckPacket(new Packet(_packetMeta, packetData));
                    }
                }
                else
                {
                    //_crypto.Reset();
                    Span<byte> packetDataView = packetData;
                    _crypto.Decrypt(ref packetDataView);
                }

                Packet packet = new Packet(_packetMeta, packetData);
                if (header != null)
                {
                    packet.Header = header;
                }

                packets.Add(packet);

                _readPacketId = false;
                read = _buffer.Position != _buffer.Size;
            }
        }

        if (_buffer.Position == _buffer.Size)
        {
            _buffer.SetPositionStart();
            _buffer.SetSize(0);
        }
        else
        {
            _position = _buffer.Position;
        }

        return packets;
    }
}