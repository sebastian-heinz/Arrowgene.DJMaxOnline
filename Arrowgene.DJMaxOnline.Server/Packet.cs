namespace Arrowgene.DJMaxOnline.Server;

public class Packet
{
    public PacketSource Source { get; set; }
    public PacketId Id { get; set; }
    public byte[] Data { get; set; }

    public Packet(PacketId id, byte[] data, PacketSource source)
    {
        Id = id;
        Source = source;
        Data = data;
    }
}