using Arrowgene.Buffers;

namespace Arrowgene.DJMaxOnline.Server.Handler;

public class ConnectReqHandler : IPacketHandler
{
    public void Handle(Client client, Packet packet)
    {
        byte[] mtSeed = new byte[32];
        Random.Shared.NextBytes(mtSeed);
        IBuffer buf = new StreamBuffer(mtSeed);
        uint sumSeed = buf.GetUInt32(28);
        buf.SetPositionStart();
        uint a = buf.ReadUInt32();
        uint b = buf.ReadUInt32();
        a = ~a;
        b = ~b;
        buf.SetPositionStart();
        buf.WriteUInt32(a);
        buf.WriteUInt32(b);
        buf.WriteBytes(new byte[] { 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, });
        Packet rsp = new Packet(PacketMeta.OnConnectAck, buf.GetAllBytes());
        rsp.Header = new byte[] { 0xCC, 0x05, 0x00, 0x4D, 0x01 };
        client.Send(rsp);

        DjMaxCrypto crypto = new DjMaxCrypto(mtSeed, sumSeed);
        client.InitCrypto(crypto);
    }

    public PacketId Id => PacketId.ConnectReq;
}