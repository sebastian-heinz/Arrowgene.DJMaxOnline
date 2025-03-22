namespace Arrowgene.DJMaxOnline.Server.Handler;

public class OnAuthenticateInAckHandler : IPacketHandler
{
    public void Handle(Client client, Packet packet)
    {
        e.Socket.Send(Convert.FromHexString(
            "10001cf9050000e29ff5ee30703c52c29980eb706844fb87577525f107e7c4e3076cc14b1bb8e484d9769cde249cbcc0eed8f0dd4e131b7d34c19e345784c392d8eb03aa05753a249454fb60df75ce95a3fa07ae64276faa69bf3524"));
        break;
    }

    public PacketId Id => PacketId.OnAuthenticateInAck;
}