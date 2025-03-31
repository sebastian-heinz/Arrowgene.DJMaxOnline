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
        //p.RunDecrypt();
        p.RunOld(args);
        //p.Run(args);
        LogProvider.Stop();
    }

    private static string RootPath =
        "C:\\Users\\nxspirit\\dev\\Arrowgene.DJMaxOnline\\Arrowgene.DJMaxOnline\\Arrowgene.DJMaxOnline.CLI\\";


    private static void LogProviderOnOnLogWrite(object? sender, LogWriteEventArgs e)
    {
        Console.WriteLine(e.Log);
        File.AppendAllText(Path.Combine(RootPath, "log.txt"), e.Log + Environment.NewLine);
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
        string yaml = File.ReadAllText(Path.Combine(RootPath, "Files/blade_stream_00.yaml"));
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
                    client.FillReadBuffer(packet.Data);
                    while (true)
                    {
                        Packet? p = client.ReadPacket();
                        if (p == null)
                        {
                            break;
                        }

                        sb.AppendLine(p.ToLog());
                        packet.ResolvedPackets.Add(p);

                        if (p.Meta.Source != PacketSource.Client)
                        {
                            Console.WriteLine($"!!!! EXPECTED SERVER PACKET {p.Meta.ToLog()}");
                        }
                    }
                }
                else if (packet.Source == PacketSource.Server)
                {
                    if (packet.Data[0] == 0x0A && packet.Data[1] == 0x00)
                    {
                        // adjust packet
                        packet.Data[0] = 0x09;
                    }

                    server.FillReadBuffer(packet.Data);
                    while (true)
                    {
                        Packet? p = server.ReadPacket();
                        if (p == null)
                        {
                            break;
                        }

                        if (p.Meta.Id == PacketId.OnConnectAck)
                        {
                            server.InitCrypto(DjMaxCrypto.FromOnConnectAckPacket(p));
                            client.InitCrypto(DjMaxCrypto.FromOnConnectAckPacket(p));
                        }

                        sb.AppendLine(p.ToLog());
                        packet.ResolvedPackets.Add(p);

                        if (p.Meta.Source != PacketSource.Server)
                        {
                            Console.WriteLine($"!!!! EXPECTED CLIENT PACKET {p.Meta.ToLog()}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        File.WriteAllText(Path.Combine(RootPath, "out.txt"), sb.ToString());
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