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
    private static readonly ILogger Logger = LogProvider.Logger(typeof(Program));

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

                        sb.AppendLine(
                            $"enc = new byte[] {{ 0x{BitConverter.ToString(PacketFactory.WritePacketOrg(p)).Replace("-", ", 0x")} }}");
                        sb.AppendLine(
                            $"dec = new byte[] {{ 0x{BitConverter.ToString(PacketFactory.WritePacket(p)).Replace("-", ", 0x")} }}");
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

                        sb.AppendLine(
                            $"enc = new byte[] {{ 0x{BitConverter.ToString(PacketFactory.WritePacketOrg(p)).Replace("-", ", 0x")} }}");
                        sb.AppendLine(
                            $"dec = new byte[] {{ 0x{BitConverter.ToString(PacketFactory.WritePacket(p)).Replace("-", ", 0x")} }}");
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
            Logger.Exception(ex);
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
        Logger.Info($"Client:{e.Socket.Identity} - packet received");
        Logger.Info(Util.HexDump(e.Data));

        StreamBuffer b = new StreamBuffer(e.Data);
        b.SetPositionStart();

        PacketId opCode = (PacketId)b.ReadUInt16();
        Logger.Info($"{opCode}");


        switch (opCode)
        {
            case PacketId.ConnectReq:
            {
                //OnConnectAck
                e.Socket.Send(new byte[]
                {
                    0x09, 0x00, 0xCC, 0x05, 0x00, 0x4D, 0x01, 0x9A, 0xDF, 0xE5, 0x67, 0x11, 0x57, 0xB2, 0xFE, 0xE8,
                    0x07, 0x06, 0x00, 0x10, 0x00, 0x00, 0x00, 0x25, 0x00, 0x2C, 0x00, 0xC0, 0x09, 0x35, 0x38, 0x0B,
                    0x3E, 0xB4, 0xA2, 0xB0, 0x11, 0x00, 0x00, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC
                });

                break;
            }
            case PacketId.AuthenticateInSndAccReq:
            {
                //OnAuthenticateInAck
                e.Socket.Send(new byte[]
                    {
                        0x10, 0x00, 0x1C, 0xF9, 0x05, 0x00, 0x00, 0xE2, 0x9F, 0xF5, 0xEE, 0x30, 0x70, 0x3C, 0x52, 0xC2,
                        0x99, 0x80, 0xEB, 0x70, 0x68, 0x44, 0xFB, 0x87, 0x57, 0x75, 0x25, 0xF1, 0x07, 0xE7, 0xC4, 0xE3,
                        0x07, 0x6C, 0xC1, 0x4B, 0x1B, 0xB8, 0xE4, 0x84, 0xD9, 0x76, 0x9C, 0xDE, 0x24, 0x9C, 0xBC, 0xC0,
                        0xEE, 0xD8, 0xF0, 0xDD, 0x4E, 0x13, 0x1B, 0x7D, 0x34, 0xC1, 0x9E, 0x34, 0x57, 0x84, 0xC3, 0x92,
                        0xD8, 0xEB, 0x03, 0xAA, 0x05, 0x75, 0x3A, 0x24, 0x94, 0x54, 0xFB, 0x60, 0xDF, 0x75, 0xCE, 0x95,
                        0xA3, 0xFA, 0x07, 0xAE, 0x64, 0x27, 0x6F, 0xAA, 0x69, 0xBF, 0x35, 0x24
                    }
                );


       


                //OnGameStartInf
                // e.Socket.Send(new byte[]
                //     {
                //         0xD3, 0x00, 0xD7, 0x00, 0x00, 0xE8, 0x07, 0x06, 0x00, 0x10, 0x00, 0x00, 0x00, 0x25, 0x00, 0x3B,
                //         0x00, 0x40, 0x4A, 0xBE, 0x2F, 0x00, 0x00, 0x00, 0x00, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
                //         0xCC
                //     }
                // );

                //OnChannelInfoInf
                e.Socket.Send(new byte[]
                    {
                        0x0B, 0x00, 0x48, 0xBB, 0x00, 0x00, 0x00, 0x80, 0x50, 0x47, 0x3F, 0xAE, 0xC9, 0x27, 0x1E, 0x0C,
                        0x22, 0x94, 0x0B, 0x09, 0xA0, 0x91, 0x80, 0x4D, 0x42, 0x19, 0xF3, 0x48, 0x7A, 0x7A, 0x12, 0xAE,
                        0xFE, 0x2C, 0x16, 0x44, 0xBF, 0xB9, 0x11, 0x3E, 0x14, 0x4C, 0xF1, 0x69, 0x75, 0xFD, 0x06, 0x8F,
                        0x6F, 0x09, 0x95, 0x0E, 0x45, 0x68, 0xF1, 0x4B, 0xB5, 0x70, 0xA7, 0x83, 0xEB, 0xFC, 0x9E, 0xF7,
                        0x80, 0x0C, 0x0E, 0x1A, 0xD4, 0x7B, 0x51, 0x0A, 0xF8, 0x5A, 0xDA, 0x55, 0xE6, 0xC0, 0x1B, 0x87,
                        0x2C, 0x26, 0xC4, 0xBB, 0x75, 0x33, 0x55, 0x01, 0xEC, 0x7E, 0x2E, 0x78, 0x28, 0xD5, 0xD0, 0x4D,
                        0xE7, 0xE1, 0x43, 0x6D, 0xB4, 0xEC, 0xA2, 0x4D, 0x5E, 0x10, 0xB4, 0xF8, 0x57, 0xF5, 0x54, 0x56,
                        0x76, 0x5D, 0x57, 0x2C, 0x34, 0x5B, 0x59, 0x39, 0x0F, 0x2D, 0x56, 0x9C, 0x8D, 0xB0, 0x1E, 0x92,
                        0x96, 0x38, 0xA8, 0x7F, 0x85, 0x3A, 0xB7, 0x88, 0xDE, 0x37, 0xFA, 0x56, 0xA0, 0x46, 0xC2, 0x02,
                        0xD0, 0x1A, 0x4C, 0x84, 0x44, 0x4F, 0xB6, 0xE9, 0x74, 0x6F, 0xB0, 0xE0, 0xA1, 0xDB, 0x3B, 0xE4,
                        0x2B, 0xE6, 0xFD, 0x52, 0xD9, 0x55, 0xBA, 0x1C, 0x7E, 0xFE, 0x43, 0x0D, 0x07, 0x17, 0xDE, 0xA8,
                        0x77, 0x0B, 0xFE, 0x66, 0x57, 0x1B, 0xA1, 0xA8, 0x93, 0x99, 0xE3
                    }
                );
                
                
                //OnUpdateUserAccountClassInf
                e.Socket.Send(new byte[]
                    {
                        0x2F, 0x00, 0x61, 0xF9, 0x05, 0x00, 0x00, 0xD1, 0xBB, 0x92, 0x1F, 0xCA, 0x85, 0x4C, 0xD3, 0xCF,
                        0xCE, 0xB8, 0x44
                    }
                );
                break;
            }
        }
    }

    private void OnClientDisconnected(object? sender, DisconnectedEventArgs e)
    {
        Logger.Info($"Client:{e.Socket.Identity} - disconnected");
    }

    private void OnClientConnected(object? sender, ConnectedEventArgs e)
    {
        Logger.Info($"Client:{e.Socket.Identity} - connected");
        e.Socket.Send(Convert.FromHexString("0300CC")); //OnPingTestInf
    }
}