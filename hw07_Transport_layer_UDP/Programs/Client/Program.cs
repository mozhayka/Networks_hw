using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SimpleUdpClient
{
    static IPEndPoint ipep, sender;
    static Socket server;
    static EndPoint Remote;
    static int recv;

    private static void Init()
    {
        ipep = new(IPAddress.Parse("127.0.0.1"), 9050);
        server = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
        sender = new(IPAddress.Any, 0);
        Remote = sender;
        Console.WriteLine("Start client");
    }

    private static void Connect()
    {        
        string message = "Connect with server";
        byte[] data = Encoding.ASCII.GetBytes(message);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);

        data = new byte[1024];
        recv = server.ReceiveFrom(data, ref Remote);
        Console.WriteLine($"Connected with {Remote}");
    }

    private static void Ask(int i)
    {
        string message = $"Ping {i + 1}, time {DateTime.Now}";
        Stopwatch stopWatch = new();
        stopWatch.Start();

        server.SendTo(Encoding.ASCII.GetBytes(message), Remote);
        try
        {
            var data = new byte[1024];
            recv = server.ReceiveFrom(data, ref Remote);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            string answer = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine($"answer: {answer}, RTT = {ts.TotalMilliseconds}");
        }
        catch
        {
            Console.WriteLine($"Request timed out");
        }
    }

    public static void Main()
    {

        Init();
        Connect();

        for (int i = 0; i < 10; i++)
        {
            Ask(i);
        }

        Console.WriteLine("Press enter to exit");
        Console.ReadLine();
        server.Close();
    }
}