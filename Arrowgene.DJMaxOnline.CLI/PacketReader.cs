using System.Globalization;
using Arrowgene.DJMaxOnline.Server;
using Arrowgene.Logging;
using YamlDotNet.Serialization;

namespace Arrowgene.DJMaxOnline;

public class YamlFile
{
    public List<YamlPeer> peers;
    public List<YamlPacket> packets;
}

public class YamlPacket
{
    public uint packet;
    public uint peer;
    public uint index;
    public string timestamp;
    public string data;
}

public class YamlPeer
{
    public uint peer;
    public string host;
    public ushort port;
}

public class PacketReader
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(PacketReader));


    public enum PacketServerType : ushort
    {
        Login = 23505,
        Game = 23705
    }

    public class PcapPacket
    {
        public PacketServerType PacketServerType { get; set; }
        public PacketSource Source { get; set; }
        public string TimeStamp { get; set; }
        public uint Index { get; set; }
        public uint Packet { get; set; }
        public byte[] Data { get; set; }
        public List<Packet> ResolvedPackets { get; set; }
    }

    public List<PcapPacket> ReadYamlPcap(string yaml)
    {
        IDeserializer yamlDeserializer = new DeserializerBuilder()
            .WithTagMapping("tag:yaml.org,2002:binary", typeof(string))
            .IgnoreUnmatchedProperties()
            .Build();
        YamlFile yamlFile = yamlDeserializer.Deserialize<YamlFile>(yaml);
        if (yamlFile.peers.Count != 2)
        {
            throw new Exception("Expected two peers");
        }

        YamlPeer serverPeer;
        YamlPeer clientPeer;
        if (yamlFile.peers[0].port is (ushort)PacketServerType.Login or (ushort)PacketServerType.Game)
        {
            serverPeer = yamlFile.peers[0];
            clientPeer = yamlFile.peers[1];
        }
        else if (yamlFile.peers[1].port is (ushort)PacketServerType.Login or (ushort)PacketServerType.Game)
        {
            serverPeer = yamlFile.peers[1];
            clientPeer = yamlFile.peers[0];
        }
        else
        {
            throw new Exception("Could not identify peer roles");
        }

        PacketServerType packetServerType;
        if (serverPeer.port == (ushort)PacketServerType.Login)
        {
            packetServerType = PacketServerType.Login;
        }
        else if (serverPeer.port == (ushort)PacketServerType.Game)
        {
            packetServerType = PacketServerType.Game;
        }
        else
        {
            throw new Exception("Could not identify server type");
        }

        List<PcapPacket> pcapPackets = new List<PcapPacket>(yamlFile.packets.Count);
        foreach (YamlPacket yamlPacket in yamlFile.packets)
        {
            if (yamlPacket.timestamp.StartsWith('0'))
            {
                Logger.Error($"Skipping broken packet {yamlPacket.packet} with invalid timestamp!");
                continue;
            }

            PcapPacket pcapPacket = new PcapPacket();
            if (yamlPacket.peer == serverPeer.peer)
            {
                pcapPacket.Source = PacketSource.Server;
            }
            else if (yamlPacket.peer == clientPeer.peer)
            {
                pcapPacket.Source = PacketSource.Client;
            }
            else
            {
                pcapPacket.Source = PacketSource.Unknown;
                Logger.Error("Failed to identify packet peer owner");
            }

            pcapPacket.PacketServerType = packetServerType;
            pcapPacket.Index = yamlPacket.index;
            pcapPacket.Data = Convert.FromBase64String(yamlPacket.data);
            pcapPacket.TimeStamp = ToReadableTimestamp(yamlPacket.timestamp);
            pcapPacket.Packet = yamlPacket.packet;
            pcapPacket.ResolvedPackets = new List<Packet>();
            pcapPackets.Add(pcapPacket);
        }

        return pcapPackets;
    }

    private static string ToReadableTimestamp(string fractionalTimestamp)
    {
        // epoch -> UTC -> Mountain Time
        double epochSeconds =
            double.Parse(fractionalTimestamp.Replace(".",
                CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
        DateTimeOffset readableTimestamp = DateTimeOffset.UnixEpoch.AddSeconds(epochSeconds);
        // If we ever get other pcap files, the time zone should instead be provided via the YAML files 
        readableTimestamp =
            TimeZoneInfo.ConvertTime(readableTimestamp, TimeZoneInfo.FindSystemTimeZoneById("America/Dawson"));
        return readableTimestamp.ToString("o");
    }
}