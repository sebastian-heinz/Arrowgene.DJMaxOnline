using Arrowgene.Buffers;

namespace Arrowgene.DJMaxOnline.Server.Handler;

public class OnAuthenticateInAckHandler : IPacketHandler
{
    public void Handle(Client client, Packet packet)
    {
        // OnAuthenticateInAck: [Id:OnAuthenticateInAck(0x10)] [Size:92] [Source:Server]
        // Header:    1C F9 05 00 00
        // 00000000   00 00 00 00 07 00 00 00  16 00 00 00 00 00 42 45   ··············BE
        // 00000010   4C 4C 45 43 4E 00 00 00  00 00 00 00 00 00 00 00   LLECN···········
        // 00000020   00 00 00 00 00 00 00 00  00 00 00 00 00 00 00 00   ················
        // 00000030   00 00 00 00 00 00 00 00  00 00 00 00 42 45 4C 4C   ············BELL
        // 00000040   45 00 00 00 00 00 00 00  00 00 00 00 00 00 00 00   E···············
        // 00000050   00 00 00 00 00                                     ·····           

        IBuffer buf = new StreamBuffer();
        buf.WriteBytes(Convert.FromHexString(
            "00000000070000001600000000004245" +
            "4C4C45434E0000000000000000000000" +
            "00000000000000000000000000000000" +
            "00000000000000000000000042454C4C" +
            "45000000000000000000000000000000" +
            "0000000000"
        ));

        Packet rsp = new Packet(PacketMeta.OnAuthenticateInAck, buf.GetAllBytes());
        rsp.Header = new byte[] { 0x1C, 0xF9, 0x05, 0x00, 0x00 };
        client.Send(rsp);
    }

    public PacketId Id => PacketId.OnAuthenticateInAck;
}