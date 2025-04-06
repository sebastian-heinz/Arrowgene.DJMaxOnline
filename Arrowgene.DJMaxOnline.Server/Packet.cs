using Arrowgene.Buffers;

namespace Arrowgene.DJMaxOnline.Server;

public class Packet
{
    public PacketMeta Meta { get; }
    public PacketId Id => Meta.Id;
    public byte[] Data { get; }
    public byte[]? Header { get; set; }
    
    public byte[]? Encrypted { get; set; }

    private IBuffer? _buffer;

    public Packet(PacketMeta meta, byte[] data)
    {
        Meta = meta;
        Data = data;
        _buffer = null;
    }

    public string ToLog()
    {
        return $"{Meta.ToLog()}" +
               Environment.NewLine +
               $"{(Header != null ? "Header:    " + BitConverter.ToString(Header).Replace("-", " ") + Environment.NewLine : "")}" +
               $"{Util.HexDump(Data)}";
    }

    public IBuffer GetBuffer()
    {
        if (_buffer == null)
        {
            _buffer = new StreamBuffer(Data);
        }

        _buffer.SetPositionStart();
        return _buffer;
    }

    public byte[] GetDataCopy()
    {
        byte[] dataCopy = new byte[Data.Length];
        Array.Copy(Data, dataCopy, dataCopy.Length);
        return dataCopy;
    }
    
}