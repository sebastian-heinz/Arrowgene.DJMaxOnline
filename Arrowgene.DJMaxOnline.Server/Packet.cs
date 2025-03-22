namespace Arrowgene.DJMaxOnline.Server;

public class Packet
{
    public PacketSource Source { get; set; }
    public PacketId Id { get; set; }
    public byte[] Data { get; set; }
    public byte[]? Header { get; set; }

    public Packet(PacketId id, byte[] data, byte[] header, PacketSource source)
    {
        Id = id;
        Source = source;
        Data = data;
        Header = header;
    }
}