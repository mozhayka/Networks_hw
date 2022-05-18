using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace IP_and_Ports
{
    class Program
    {
        public static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", address));
        }

        static IPAddress GetIP()
        {
            string Host = Dns.GetHostName();
            var IP = Dns.GetHostEntry(Host).AddressList[1];
            return IP;
        }

        static List<int> GetPorts(int min, int max, IPAddress ip)
        {
            var ans = Enumerable.Range(min, max - min + 1)
                .Where(n => !IPGlobalProperties
                    .GetIPGlobalProperties()
                    .GetActiveTcpListeners()
                    .Any(l => l.Address.ToString() == ip.ToString() && l.Port == n))
                .ToList();
            return ans;
        }

        static void Main(string[] args)
        {
            var IP = GetIP();
            var mask = GetSubnetMask(IP);
            var ports = GetPorts(130, 140, IP);

            Console.WriteLine($"IP {IP}\nmask {mask}\nFree ports");
            ports.ForEach(x => Console.WriteLine(x));
        }
    }
}
