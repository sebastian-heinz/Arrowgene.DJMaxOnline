using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.DJMaxOnline.Server;

public class Client
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(PacketFactory));

    public Client(ITcpSocket socket, PacketFactory packetFactory)
    {
        _socket = socket;
        _packetFactory = packetFactory;
        Identity = socket.Identity;
        UpdateIdentity();
    }

    private readonly ITcpSocket _socket;
    private readonly PacketFactory _packetFactory;


    public string Identity { get; protected set; }

    public DateTime PingTime { get; set; }


    public void UpdateIdentity()
    {
        // call when user info set/changed
        //string newIdentity = $"[GameClient@{Socket.Identity}]";
        //if (Account != null)
        //{
        //    newIdentity += $"[Acc:({Account.Id}){Account.NormalName}]";
        //}

        //if (Character != null)
        //{
        //    newIdentity += $"[Cha:({Character.CharacterId}){Character.FirstName} {Character.LastName}]";
        //}

        //Identity = newIdentity;
    }

    public void Close()
    {
        _socket.Close();
    }

    public List<Packet> Receive(byte[] data)
    {
        List<Packet> packets;
        try
        {
            packets = _packetFactory.Read(data);
        }
        catch (Exception ex)
        {
            Logger.Exception(this, ex);
            packets = new List<Packet>();
        }

        foreach (Packet packet in packets)
        {
            Logger.LogPacket(this, packet);
        }

        return packets;
    }

    public void Send(Packet packet)
    {
        byte[] data;
        try
        {
            data = _packetFactory.Write(packet);
        }
        catch (Exception ex)
        {
            Logger.Exception(this, ex);
            return;
        }

        SendRaw(data);
        Logger.LogPacket(this, packet);
    }

    /// <summary>
    /// Sends raw bytes to the client, without any further processing
    /// </summary>
    public void SendRaw(byte[] data)
    {
        _socket.Send(data);
    }
}