using System.Net;
using Arrowgene.Buffers;
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
        p.Run(args);
        LogProvider.Stop();
    }

    private static void LogProviderOnOnLogWrite(object? sender, LogWriteEventArgs e)
    {
        Console.WriteLine(e.Log);
    }

    public void Run(string[] args)
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

        uint opCode = b.ReadUInt16();
        Console.WriteLine(opCode);

        switch (opCode)
        {
            case 0x0A:
            {
                e.Socket.Send(Convert.FromHexString(
                    "0900cc05004d019adfe5671157b2fee80706001000000025002c00c00935380b3eb4a2b0110000cccccccccccccccc"));
                break;
            }
            case 0x11:
            {
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
        e.Socket.Send(Convert.FromHexString("0300CC"));
    }
}