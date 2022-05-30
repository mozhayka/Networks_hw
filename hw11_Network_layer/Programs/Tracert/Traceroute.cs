using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tracert
{
    public class TraceRoute
    {
        private const int MAX_HOPS_NUMBER = 30;
        private const int MAX_BAD_ATTEMPTS = 15;

        int packet_cnt = 3;
        string url;

        ICMP packet;
        Socket host;
        IPHostEntry iphe;
        IPEndPoint iep;
        EndPoint ep;

        public TraceRoute(string url)
        {
            this.url = url;

            packet = new();
            host = new(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            iphe = Dns.GetHostEntry(url); 
            iep = new(iphe.AddressList[0], 0);
            ep = iep;
        }

        public void Run(int packet_cnt = 3)
        {
            this.packet_cnt = packet_cnt;
            Console.WriteLine($"tracert {url}\nMax hops {MAX_HOPS_NUMBER}\n");
            host.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
            for (int i = 1; i < MAX_HOPS_NUMBER; i++)
            {
                if (Step(i))
                    break;
            }
            host.Close();
        }

        private bool Step(int i)
        {
            host.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.IpTimeToLive, i);

            Console.Write($"{i}\t");
            int time = -1;
            bool res = false;

            for (int j = 0; j < packet_cnt; j++)
            {
                SendReceive(out time, out res);
                Console.Write($"{Time(time)}\t");
            }

            string adres = new(ep.ToString().TakeWhile(x => x != ':').ToArray());
            Console.Write($"{adres}\t");
            Console.Write($"{GetHostName(adres)}");
            Console.WriteLine();

            if (res == false)
            {
                if (time == -1)
                    Console.WriteLine("Unable to contact remote host");
                else
                    Console.WriteLine($"{adres} ({url}) reached in {i} hops, {time} ms.");
            }
            return !res;
        }

        private string Time(int time)
        {
            return (time == -1 ? "*" : $"{time} ms");
        }

        private string GetHostName(string adres)
        {
            try
            {
                return Dns.GetHostEntry(adres).HostName;
            }
            catch
            {
                return "Unknown";
            }
        }


        int badcount = 0;
        private void SendReceive(out int time, out bool result)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            host.SendTo(packet.GetBytes(), packet.MessageSize + 4, SocketFlags.None, iep);

            try
            {
                var data = new byte[1024];
                var recv = host.ReceiveFrom(data, ref ep);

                stopWatch.Stop();
                time = (int) stopWatch.ElapsedMilliseconds;

                ICMP response = new(data, recv);

                if (response.Type == 0)
                    result = false;
                else
                    result = true;

                badcount = 0;
            }
            catch (SocketException)
            {
                badcount++;
                time = -1;

                if (badcount >= MAX_BAD_ATTEMPTS)
                    result = false;
                else
                    result = true;
            }
        }
    }
}
