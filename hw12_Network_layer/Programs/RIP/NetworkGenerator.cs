using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIP
{
    class NetworkGenerator
    {
        public List<string> adreses;
        public Dictionary<string, List<string>> connections;
        Random rand = new();

        public NetworkGenerator(int v, int e)
        {
            GenIPs(v);
            Console.WriteLine("Adreses");
            adreses.ForEach(x => Console.WriteLine(x));
            Console.WriteLine();

            GenConnections(e);
            Console.WriteLine("Connections");
            foreach (var from in connections.Keys)
            {
                foreach (var to in connections[from])
                {
                    Console.WriteLine($"{from} -> {to}");
                }
            }
            Console.WriteLine();
        }

        private void GenIPs(int v)
        {
            adreses = new();
            for (int i = 0; i < v; i++)
            {
                string IP = GenIP();
                while (adreses.Contains(IP))
                    IP = GenIP();
                adreses.Add(IP);
            }
        }

        private string GenIP()
        {
            return $"{rand.Next(0, 255)}.{rand.Next(0, 255)}.{rand.Next(0, 255)}.{rand.Next(0, 255)}";
        }

        private void GenConnections(int e)
        {
            connections = new();
            foreach (var adr in adreses)
                connections[adr] = new();

            int v = adreses.Count;
            for (int i = 0; i < e; i++)
            {
                int from = rand.Next(0, v);
                int to = rand.Next(0, v);
                while (from == to || connections[adreses[from]].Contains(adreses[to]))
                {
                    from = rand.Next(0, v);
                    to = rand.Next(0, v);
                }
                connections[adreses[from]].Add(adreses[to]);
                connections[adreses[to]].Add(adreses[from]);
            }
        }
    }
}
