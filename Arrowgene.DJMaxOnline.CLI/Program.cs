using System.Net;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.DJMaxOnline.Server;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Consumer.EventConsumption;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.DJMaxOnline;

public class Program
{
    public static void Main(string[] args)
    {
        LogProvider.OnLogWrite += LogProviderOnOnLogWrite;
        LogProvider.Start();
        Program p = new Program();
        p.RunDecrypt();
        // p.Run(args);
        LogProvider.Stop();
    }

    private static void LogProviderOnOnLogWrite(object? sender, LogWriteEventArgs e)
    {
        Console.WriteLine(e.Log);
    }

    public void Run(string[] args)
    {
        Setting setting = new Setting();
        DjMaxServer server = new DjMaxServer(setting);
        server.Start();
        bool isRunning = true;
        while (isRunning)
        {
            ConsoleKeyInfo cki = Console.ReadKey();
            if (cki.Key == ConsoleKey.Escape)
            {
                isRunning = false;
            }
        }

        server.Stop();
    }

    public void RunDecrypt()
    {
        string yaml = File.ReadAllText(
            "/Users/shiba/dev/Arrowgene.DJMaxOnline/Arrowgene.DJMaxOnline.CLI/Files/blade_stream_00.yaml"
        );
        PacketReader r = new PacketReader();
        List<PacketReader.PcapPacket> packets = r.ReadYamlPcap(yaml);

        PacketFactory server = new PacketFactory();
        PacketFactory client = new PacketFactory();
        StringBuilder sb = new StringBuilder();

        try
        {
            foreach (PacketReader.PcapPacket packet in packets)
            {
                if (packet.Source == PacketSource.Client)
                {
                    packet.ResolvedPackets = client.Read(packet.Data);
                }
                else if (packet.Source == PacketSource.Server)
                {
                    if (packet.Data[0] == 0x0A && packet.Data[1] == 0x00)
                    {
                        // adjust packet
                        packet.Data[0] = 0x09;
                    }

                    packet.ResolvedPackets = server.Read(packet.Data);

                    foreach (Packet p in packet.ResolvedPackets)
                    {
                        if (p.Id == PacketId.OnConnectAck)
                        {
                            client.InitCrypto(DjMaxCrypto.FromOnConnectAckPacket(p));
                        }
                    }
                }

                foreach (Packet p in packet.ResolvedPackets)
                {
                    sb.AppendLine(p.ToLog());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        File.WriteAllText("/Users/shiba/dev/Arrowgene.DJMaxOnline/Arrowgene.DJMaxOnline.CLI/out.txt",
            sb.ToString());
    }

    public void RunOld(string[] args)
    {
        EventConsumer eventConsumer = new EventConsumer();
        eventConsumer.ClientConnected += OnClientConnected;
        eventConsumer.ClientDisconnected += OnClientDisconnected;
        eventConsumer.ReceivedPacket += OnReceivedPacket;
        AsyncEventServer server = new AsyncEventServer(IPAddress.Any, 1234, eventConsumer);
        server.Start();

        bool isRunning = true;
        while (isRunning)
        {
            ConsoleKeyInfo cki = Console.ReadKey();
            if (cki.Key == ConsoleKey.Escape)
            {
                isRunning = false;
            }
        }

        server.Stop();
    }

    private void OnReceivedPacket(object? sender, ReceivedPacketEventArgs e)
    {
        Console.WriteLine($"Client:{e.Socket.Identity} - packet received");
        Console.WriteLine(Util.HexDump(e.Data));

        StreamBuffer b = new StreamBuffer(e.Data);
        b.SetPositionStart();

        PacketId opCode = (PacketId)b.ReadUInt16();
        Console.WriteLine(opCode);


        switch (opCode)
        {
            case PacketId.ConnectReq:
            {
                //OnConnectAck
                e.Socket.Send(Convert.FromHexString(
                    "0900cc05004d019adfe5671157b2fee80706001000000025002c00c00935380b3eb4a2b0110000cccccccccccccccc"));
                break;
            }
            case PacketId.AuthenticateInSndAccReq:
            {
                //OnAuthenticateInAck
                e.Socket.Send(Convert.FromHexString(
                    "10001cf9050000e29ff5ee30703c52c29980eb706844fb87577525f107e7c4e3076cc14b1bb8e484d9769cde249cbcc0eed8f0dd4e131b7d34c19e345784c392d8eb03aa05753a249454fb60df75ce95a3fa07ae64276faa69bf3524"));
                break;
            }
        }
    }

    private void OnClientDisconnected(object? sender, DisconnectedEventArgs e)
    {
        Console.WriteLine($"Client:{e.Socket.Identity} - disconnected");
    }

    private void OnClientConnected(object? sender, ConnectedEventArgs e)
    {
        Console.WriteLine($"Client:{e.Socket.Identity} - connected");
        e.Socket.Send(Convert.FromHexString("0300CC")); //OnPingTestInf
    }
}