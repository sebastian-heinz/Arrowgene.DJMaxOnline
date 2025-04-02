using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.DJMaxOnline.Server;

public class PacketFactory
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(PacketFactory));

    private const int PacketHeaderSize = 5;
    private const int PacketIdSize = 2;

    private readonly IBuffer _buffer;
    private bool _readPacketId;
    private int _dataSize;
    private DjMaxCrypto? _crypto;
    private PacketMeta _packetMeta;

    public PacketFactory()
    {
        _readPacketId = false;
        _dataSize = 0;
        _buffer = new StreamBuffer();
        _crypto = null;
    }

    public void InitCrypto(DjMaxCrypto crypto)
    {
        _crypto = crypto;
    }
    
    public static byte[] WritePacket(Packet packet, DjMaxCrypto? crypto = null)
    {
        byte[] packetData = packet.Data;

        if (crypto != null)
        {
            Span<byte> packetDataView = packetData;
            crypto.Encrypt(ref packetDataView);
        }

        IBuffer buffer = new StreamBuffer();
        buffer.WriteUInt16((ushort)packet.Id);
        if (packet.Header != null)
        {
            buffer.WriteBytes(packet.Header);
        }

        buffer.WriteBytes(packetData);
        byte[] b = buffer.GetAllBytes();
        if (b.Length != packet.Meta.Size)
        {
            Logger.Error(
                $"Packet Size mismatch. Expected: {packet.Meta.Size}, actual: {b.Length}.{Environment.NewLine}" +
                $"PacketMeta:{packet.Meta.ToLog()} Raw:{Environment.NewLine}" +
                Util.HexDump(b)
            );
        }

        return b;
    }

    public static byte[] WritePacketOrg(Packet packet, DjMaxCrypto? crypto = null)
    {
        byte[] packetData = packet.Org;

        if (crypto != null)
        {
            Span<byte> packetDataView = packetData;
            crypto.Encrypt(ref packetDataView);
        }

        IBuffer buffer = new StreamBuffer();
        buffer.WriteUInt16((ushort)packet.Id);
        if (packet.Header != null)
        {
            buffer.WriteBytes(packet.Header);
        }

        buffer.WriteBytes(packetData);
        byte[] b = buffer.GetAllBytes();
        if (b.Length != packet.Meta.Size)
        {
            Logger.Error(
                $"Packet Size mismatch. Expected: {packet.Meta.Size}, actual: {b.Length}.{Environment.NewLine}" +
                $"PacketMeta:{packet.Meta.ToLog()} Raw:{Environment.NewLine}" +
                Util.HexDump(b)
            );
        }

        return b;
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
        if (packet.Header != null)
        {
            buffer.WriteBytes(packet.Header);
        }

        buffer.WriteBytes(packetData);
        byte[] b = buffer.GetAllBytes();
        if (b.Length != packet.Meta.Size)
        {
            Logger.Error(
                $"Packet Size mismatch. Expected: {packet.Meta.Size}, actual: {b.Length}.{Environment.NewLine}" +
                $"PacketMeta:{packet.Meta.ToLog()} Raw:{Environment.NewLine}" +
                Util.HexDump(b)
            );
        }

        return b;
    }

    public void FillReadBuffer(byte[] data)
    {
        if (_buffer.Position == _buffer.Size)
        {
            _buffer.SetPositionStart();
            _buffer.SetSize(0);
            _buffer.WriteBytes(data);
            _buffer.SetPositionStart();
        }
        else
        {
            int pos = _buffer.Position;
            _buffer.SetPositionEnd();
            _buffer.WriteBytes(data);
            _buffer.Position = pos;
        }
    }

    public Packet? ReadPacket()
    {
        if (!_readPacketId && _buffer.Size - _buffer.Position >= PacketIdSize)
        {
            ushort packetIdNum = _buffer.ReadUInt16();

            if (!Enum.IsDefined(typeof(PacketId), packetIdNum))
            {
                // TODO err
                Logger.Error($"packetIdNum: {packetIdNum}(0x{packetIdNum:X}) is not a defined PacketId");
            }

            PacketId packetId = (PacketId)packetIdNum;

            if (!PacketMeta.TryGet(packetId, out _packetMeta))
            {
                // TODO err
                Logger.Error($"PacketMeta not defined for packetId: {packetId}(0x{packetIdNum:X})");
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
                _dataSize -= PacketHeaderSize;
            }

            byte[] packetData = _buffer.ReadBytes(_dataSize);
            byte[] org = new byte[packetData.Length];
            packetData.CopyTo(org, 0);
            if (_crypto != null)
            {
                Span<byte> packetDataView = packetData;
                _crypto.Decrypt(ref packetDataView);

                if (_packetMeta.Id == PacketId.AuthenticateInSndAccReq)
                {
                    IBuffer b = new StreamBuffer(packetData);
                    b.SetPositionStart();
                    byte[] user = b.ReadBytes(21);
                    byte[] pw = b.ReadBytes(11);
                    byte[] mtSeed = b.ReadBytes(32);
                    uint crc32 = Crc32.GetHash(mtSeed);
                    byte[] crc = BitConverter.GetBytes(crc32);
                    
                    for (int i = 0; i < user.Length; i++)
                    {
                        user[i] = (byte)(user[i] - crc[i % crc.Length]);
                        user[i] = (byte)~user[i];
                    }

                    Console.WriteLine(Util.HexDump(user));
                }
            }

            Packet packet = new Packet(_packetMeta, packetData);
            if (header != null)
            {
                packet.Header = header;
            }
            packet.Org = org;

            _readPacketId = false;
            return packet;
        }

        return null;
    }

    public List<Packet> ReadPackets()
    {
        List<Packet> packets = new List<Packet>();
        while (true)
        {
            Packet? p = ReadPacket();
            if (p == null)
            {
                break;
            }

            packets.Add(p);
        }

        return packets;
    }
}