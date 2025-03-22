using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.DJMaxOnline.Server;

public class ServerLogger : Logger
{
    public override void Initialize(string identity, string name, Action<Log> write)
    {
        base.Initialize(identity, name, write);
    }

    public override void Configure(object loggerTypeConfig, object identityConfig)
    {
        base.Configure(loggerTypeConfig, identityConfig);
        //_serverSetting = identityConfig as ServerSetting;
    }

    public void Hex(byte[] data)
    {
        Info($"\n{Util.HexDump(data)}");
    }

    public void Info(Client client, string message)
    {
        Info($"{client.Identity} {message}");
    }

    public void Debug(Client client, string message)
    {
        Debug($"{client.Identity} {message}");
    }

    public void Error(Client client, string message)
    {
        Error($"{client.Identity} {message}");
    }

    public void Exception(Client client, Exception exception)
    {
        Write(LogLevel.Error, $"{client.Identity} {exception}", exception);
    }

    public void Info(ITcpSocket socket, string message)
    {
        Info($"[{socket.Identity}] {message}");
    }

    public void Debug(ITcpSocket socket, string message)
    {
        Debug($"[{socket.Identity}] {message}");
    }

    public void Error(ITcpSocket socket, string message)
    {
        Error($"[{socket.Identity}] {message}");
    }

    public void Exception(ITcpSocket socket, Exception exception)
    {
        Write(LogLevel.Error, $"{socket.Identity} {exception}", exception);
    }

    public void LogPacket(Client client, Packet packet)
    {
        Write(LogLevel.Debug, $"{client.Identity}{Environment.NewLine}{packet}", packet);
    }

    public void LogUnhandledPacket(Client client, Packet packet)
    {
        throw new NotImplementedException();
    }

    public void LogPacketError(Client client, Packet packet)
    {
        throw new NotImplementedException();
    }
}