using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.DJMaxOnline.Server.Handler;

public class ConnectReqHandler : IPacketHandler
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectReqHandler));

    public void Handle(Client client, Packet packet)
    {
        byte[] mtSeed = new byte[32];
        Random.Shared.NextBytes(mtSeed);
        uint crc32 = Crc32.GetHash(mtSeed);
        Logger.Info($"Crc32:{crc32:X8}");
        
        IBuffer buf = new StreamBuffer(mtSeed);
        uint sumSeed = buf.GetUInt32(28);
        buf.SetPositionStart();
        uint a = buf.ReadUInt32();
        uint b = buf.ReadUInt32();
        Logger.Info($"A:{a:x8} B:{b:x8}");
        a = ~a;
        b = ~b;
        Logger.Info($"A:{a:x8} B:{b:x8}");
        buf.SetPositionStart();
        buf.WriteUInt32(a);
        buf.WriteUInt32(b);
        buf.Position = 32;
        buf.WriteBytes(new byte[] { 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, });
        Packet rsp = new Packet(PacketMeta.OnConnectAck, buf.GetAllBytes());
        rsp.Header = new byte[] { 0xCC, 0x05, 0x00, 0x4D, 0x01 };
        client.Send(rsp);
        
        
    

        DjMaxCrypto crypto = new DjMaxCrypto(mtSeed, sumSeed);
        client.InitCrypto(crypto);
    }

    public PacketId Id => PacketId.ConnectReq;
}