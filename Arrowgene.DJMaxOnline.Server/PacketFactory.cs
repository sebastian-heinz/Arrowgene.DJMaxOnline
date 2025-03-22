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

    public PacketFactory()
    {
        Reset();
    }

    public byte[] Write(Packet packet)
    {
        packet.Source = PacketSource.Server;
        byte[] data = packet.Data;
        if (data == null)
        {
            Logger.Error($"data == null, tried to write invalid data");
            return null;
        }

        IBuffer buffer = new StreamBuffer();
        buffer.WriteUInt16((ushort)packet.Id);
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
                byte[] header = _buffer.ReadBytes(PacketHeaderSize);
                _readHeader = true;
                _dataSize = _buffer.Size - _buffer.Position;
            }

            if (_readHeader && _buffer.Size - _buffer.Position >= _dataSize)
            {
                byte[] encryptedPacketData = _buffer.ReadBytes(_dataSize);
                byte[] packetData = Decrypt(encryptedPacketData);

                byte[] payload;
                PacketId packetId;
                uint packetCount;

                Packet packet = new Packet(packetId, payload, packetSource, packetCount);

                packets.Add(packet);

                _readHeader = false;
                read = _buffer.Position != _buffer.Size;
            }
            else
            {
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