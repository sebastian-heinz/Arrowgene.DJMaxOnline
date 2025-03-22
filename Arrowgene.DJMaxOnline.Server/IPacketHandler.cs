namespace Arrowgene.DJMaxOnline.Server;

public interface IPacketHandler
{
    void Handle(Client client, Packet packet);
    PacketId Id { get; }
}