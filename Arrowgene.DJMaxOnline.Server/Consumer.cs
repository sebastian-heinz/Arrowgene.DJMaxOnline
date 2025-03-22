using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;

namespace Arrowgene.DJMaxOnline.Server;

public class Consumer : ThreadedBlockingQueueConsumer
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(Consumer));

    private readonly Dictionary<PacketId, IPacketHandler> _packetHandlerLookup;
    private readonly Dictionary<ITcpSocket, Client> _clients;
    private readonly object _lock;
    private readonly Setting _setting;
    public Action<Client> ClientDisconnected;
    public Action<Client> ClientConnected;

    public Consumer(Setting setting)
        : base(setting.SocketSettings, setting.Name)
    {
        _setting = setting;
        _lock = new object();
        _clients = new Dictionary<ITcpSocket, Client>();
        _packetHandlerLookup = new Dictionary<PacketId, IPacketHandler>();
    }

    public void Clear()
    {
        _packetHandlerLookup.Clear();
    }

    public void AddHandler(IPacketHandler packetHandler)
    {
        if (!_packetHandlerLookup.TryAdd(packetHandler.Id, packetHandler))
        {
            Logger.Error($"PacketHandlerId: {packetHandler.Id} already exists");
        }
    }

    protected override void HandleReceived(ITcpSocket socket, byte[] data)
    {
        if (!socket.IsAlive)
        {
            return;
        }

        Client client;
        lock (_lock)
        {
            if (!_clients.TryGetValue(socket, out client))
            {
                Logger.Error(socket, "Client does not exist in lookup");
                return;
            }
        }

        List<Packet> packets = client.Receive(data);
        foreach (Packet packet in packets)
        {
            HandlePacket(client, packet);
        }
    }

    private void HandlePacket(Client client, Packet packet)
    {
        if (!_packetHandlerLookup.TryGetValue(packet.Id, out var packetHandler))
        {
            Logger.LogUnhandledPacket(client, packet);
            return;
        }

        try
        {
            packetHandler.Handle(client, packet);
        }
        catch (Exception ex)
        {
            Logger.Exception(client, ex);
            Logger.LogPacketError(client, packet);
        }
    }

    protected override void HandleDisconnected(ITcpSocket socket)
    {
        Client client;
        lock (_lock)
        {
            if (!_clients.ContainsKey(socket))
            {
                Logger.Error(socket, $"Disconnected client does not exist in lookup");
                return;
            }

            client = _clients[socket];
            _clients.Remove(socket);
        }

        Action<Client> onClientDisconnected = ClientDisconnected;
        if (onClientDisconnected != null)
        {
            try
            {
                onClientDisconnected.Invoke(client);
            }
            catch (Exception ex)
            {
                Logger.Exception(client, ex);
            }
        }

        Logger.Info($"Disconnected: {client.Identity}");
    }

    protected override void HandleConnected(ITcpSocket socket)
    {
        Client client = new Client(socket, new PacketFactory());
        lock (_lock)
        {
            _clients.Add(socket, client);
        }

        Logger.Info($"Connected: {client.Identity}");

        Action<Client> onClientConnected = ClientConnected;
        if (onClientConnected != null)
        {
            try
            {
                onClientConnected.Invoke(client);
            }
            catch (Exception ex)
            {
                Logger.Exception(client, ex);
            }
        }
    }
}