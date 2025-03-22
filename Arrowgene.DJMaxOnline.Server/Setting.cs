using System.Net;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.DJMaxOnline.Server;

public class Setting
{
    public string Name { get; }
    public AsyncEventSettings SocketSettings { get; }
    public IPAddress ListenIpAddress { get; }
    public ushort ServerPort { get; }
}