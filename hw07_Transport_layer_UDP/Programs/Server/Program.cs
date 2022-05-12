using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server
{
    static IPEndPoint ipep, sender;
    static Socket newsock;
    static EndPoint Remote;
    static int recv;

    private static void Init()
    {
        ipep = new(IPAddress.Any, 9050);

        newsock = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        newsock.Bind(ipep);

        sender = new(IPAddress.Any, 0);
        Remote = sender;        
        Console.WriteLine("Waiting for a client");
    }

    private static void Connect()
    {
        var data = new byte[1024];
        recv = newsock.ReceiveFrom(data, ref Remote);
        Console.WriteLine($"Connected with {Remote}:");

        string message = "Connect with client";
        data = Encoding.ASCII.GetBytes(message);
        newsock.SendTo(data, data.Length, SocketFlags.None, Remote);
    }

    private static void Answer()
    {
        Random rand = new();
        var data = new byte[1024];
        recv = newsock.ReceiveFrom(data, ref Remote);
        string message = Encoding.ASCII.GetString(data, 0, recv);

        //Console.WriteLine(message);
        if (rand.NextDouble() > 0.2)
        {
            var answer = Encoding.ASCII.GetBytes(message.ToUpper());
            newsock.SendTo(answer, recv, SocketFlags.None, Remote);
        }
    }

    public static void Main()
    {    
        Init();
        Connect();
        while (true)
        {
            Answer();
        }
    }
}
