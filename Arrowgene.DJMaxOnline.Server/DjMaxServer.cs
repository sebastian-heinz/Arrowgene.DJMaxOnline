using Arrowgene.DJMaxOnline.Server;
using Arrowgene.DJMaxOnline.Server.Handler;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

public class DjMaxServer
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DjMaxServer));
    private readonly Consumer _consumer;
    private readonly AsyncEventServer _server;
    private readonly Setting _setting;

    public DjMaxServer(Setting setting)
    {
        _setting = setting;
        _consumer = new Consumer(_setting);
        _consumer.ClientConnected += ClientConnected;
        _consumer.ClientDisconnected += ClientDisconnected;

        _server = new AsyncEventServer(
            _setting.ListenIpAddress,
            _setting.ServerPort,
            _consumer,
            _setting.AsyncEventSettings
        );

        ClientLookup = new ClientLookup();


        LoadPacketHandler();
    }

    public ClientLookup ClientLookup { get; }

    public void Start()
    {
        Logger.Info("Starting...");
        _server.Start();
        Logger.Info("Server started.");
    }

    public void Stop()
    {
        Logger.Info("Stopping...");
        _server.Stop();
        Logger.Info("Stopped");
    }

    private void ClientConnected(Client client)
    {
        Packet p = new Packet(PacketId.OnPingTestInf, new byte[]
        {
            0xCC
        }, null, PacketSource.Server);
        client.Send(p);
    }

    private void ClientDisconnected(Client client)
    {
        ClientLookup.Remove(client);
    }

    private void LoadPacketHandler()
    {
        _consumer.AddHandler(new ConnectReqHandler());
        _consumer.AddHandler(new OnAuthenticateInAckHandler());
    }
}