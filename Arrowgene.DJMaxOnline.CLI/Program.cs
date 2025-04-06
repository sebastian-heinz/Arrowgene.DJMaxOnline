using System.Text;
using Arrowgene.DJMaxOnline.Server;
using Arrowgene.Logging;

namespace Arrowgene.DJMaxOnline;

public class Program
{
    public static void Main(string[] args)
    {
        LogProvider.OnLogWrite += LogProviderOnOnLogWrite;
        LogProvider.Start();
        Program p = new Program();
        // p.RunDecrypt();
        p.Run(args);
        LogProvider.Stop();
    }

    private static readonly ILogger Logger = LogProvider.Logger(typeof(Program));

    // TODO make this resolve dynamic and copy ./Files to output/build dir
    private static string RootPath =
        "C:\\Users\\nxspirit\\dev\\Arrowgene.DJMaxOnline\\Arrowgene.DJMaxOnline\\Arrowgene.DJMaxOnline.CLI\\";
    //private static string RootPath =
    //  "/Users/shiba/dev/Arrowgene.DJMaxOnline/Arrowgene.DJMaxOnline.CLI/";


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
                            $"enc = new byte[] {{ 0x{BitConverter.ToString(p.Encrypted).Replace("-", ", 0x")} }}");
                        sb.AppendLine(
                            $"dec = new byte[] {{ 0x{BitConverter.ToString(p.Data).Replace("-", ", 0x")} }}");
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
                            $"enc = new byte[] {{ 0x{BitConverter.ToString(p.Encrypted).Replace("-", ", 0x")} }}");
                        sb.AppendLine(
                            $"dec = new byte[] {{ 0x{BitConverter.ToString(p.Data).Replace("-", ", 0x")} }}");
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
}