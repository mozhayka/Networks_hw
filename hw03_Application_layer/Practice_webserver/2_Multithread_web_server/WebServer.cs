using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WebServer
{
    class WebServer
    {
        static Socket InitSocket(string address = "127.0.0.1", int port = 8081)
        {
            var ia = IPAddress.Parse(address);
            var ie = new IPEndPoint(ia, port);

            Console.WriteLine($"In Browser run: http://{ie}/HelloWorld.txt");
            Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ie);
            socket.Listen();
            return socket;
        }

        static void RequestProcessing(Socket socket)
        {
            Request request = new();
            while (true)
            {
                Socket connectionSocket = socket.Accept();
                Thread thread = new(() => request.Answer(connectionSocket));
                thread.Start();
            }
        }

        static void Main(string[] args)
        {
            var socket = InitSocket();
            RequestProcessing(socket);
        }
    }
}