namespace Arrowgene.DJMaxOnline.Server.Handler;

public class KeepAuthenticateInReqHandler : IPacketHandler
{
    public void Handle(Client client, Packet packet)
    {
        
    }

    public PacketId Id => PacketId.OnAuthenticateInAck;
}