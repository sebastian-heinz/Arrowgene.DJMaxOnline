namespace Arrowgene.DJMaxOnline.Server;

public class Packet
{
    public PacketMeta Meta { get; }
    public PacketId Id => Meta.Id;
    public byte[] Data { get; }

    public Packet(PacketMeta meta, byte[] data)
    {
        Meta = meta;
        Data = data;
    }

    public string ToLog()
    {
        return $"{Meta.ToLog()}" +
               Environment.NewLine +
               $"{Util.HexDump(Data)}";
    }
}