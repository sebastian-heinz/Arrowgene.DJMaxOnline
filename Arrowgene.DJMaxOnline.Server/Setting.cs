using System.Net;
using System.Runtime.Serialization;
using Arrowgene.Networking;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.DJMaxOnline.Server;

public class Setting
{
    [DataMember(Order = 1)] public string Name { get; set; }

    [DataMember(Order = 2)] public IPAddress ListenIpAddress { get; set; }

    [DataMember(Order = 3)] public ushort ServerPort { get; set; }

    [DataMember(Order = 10)] public AsyncEventSettings AsyncEventSettings { get; set; }

    public Setting()
    {
        Name = "DjMaxServer";
        ListenIpAddress = IPAddress.Any;
        ServerPort = 1234;
        AsyncEventSettings = new AsyncEventSettings();
    }

    public Setting(Setting setting)
    {
        Name = setting.Name;
        ListenIpAddress = setting.ListenIpAddress;
        ServerPort = setting.ServerPort;
        AsyncEventSettings = new AsyncEventSettings(setting.AsyncEventSettings);
    }
}