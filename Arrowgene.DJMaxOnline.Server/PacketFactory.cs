using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.DJMaxOnline.Server;

public class PacketFactory
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(PacketFactory));

    private const int PacketHeaderSize = 7;

    private bool _readHeader;
    private ushort _dataSize;
    private int _position;
    private IBuffer _buffer;


    private DjMaxCrypto? _crypto;
    private PacketId _packetId;
    private byte[] _header;

    public PacketFactory()
    {
        _crypto = null;
        Reset();
    }


    public void InitCrypto(byte[] mtSeed, uint delta)
    {
        _crypto = new DjMaxCrypto(mtSeed, delta);
    }

    public byte[] Write(Packet packet)
    {
        packet.Source = PacketSource.Server;
        byte[] packetData = packet.Data;
        if (packetData == null)
        {
            Logger.Error($"data == null, tried to write invalid data");
            return null;
        }

        if (_crypto != null)
        {
            Span<byte> packetDataView = packetData;
            _crypto.Encrypt(ref packetDataView);
        }

        IBuffer buffer = new StreamBuffer();
        buffer.WriteUInt16((ushort)packet.Id);
        buffer.WriteBytes(packetData);
        if (packet.Header != null)
        {
            buffer.WriteBytes(packet.Header);
        }
        return buffer.GetAllBytes();
    }

    public List<Packet> Read(byte[] data)
    {
        List<Packet> packets = new List<Packet>();
        if (_buffer == null)
        {
            _buffer = new StreamBuffer(data);
        }
        else
        {
            _buffer.SetPositionEnd();
            _buffer.WriteBytes(data);
        }

        _buffer.Position = _position;

        bool read = true;
        while (read)
        {
            read = false;
            if (!_readHeader && _buffer.Size - _buffer.Position >= PacketHeaderSize)
            {
                _packetId = (PacketId)_buffer.ReadUInt16();
                _header = _buffer.ReadBytes(PacketHeaderSize - 2);
                _readHeader = true;
                _dataSize = (ushort)(_buffer.Size - _buffer.Position);
            }

            if (_readHeader && _buffer.Size - _buffer.Position >= _dataSize)
            {
                byte[] packetData = _buffer.ReadBytes(_dataSize);
                if (_crypto != null)
                {
                    Span<byte> packetDataView = packetData;
                    _crypto.Decrypt(ref packetDataView);
                }

                Packet packet = new Packet(_packetId, packetData, _header, PacketSource.Client);
                packets.Add(packet);

                _readHeader = false;
                read = _buffer.Position != _buffer.Size;
            }
        }

        if (_buffer.Position == _buffer.Size)
        {
            Reset();
        }
        else
        {
            _position = _buffer.Position;
        }

        return packets;
    }


    private void Reset()
    {
        _readHeader = false;
        _dataSize = 0;
        _position = 0;
        _buffer = null;
    }
}