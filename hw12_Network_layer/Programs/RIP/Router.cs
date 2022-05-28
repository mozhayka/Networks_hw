using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIP
{
    class Router
    {
        public readonly string start_adr;
        Dictionary<string, string> next_hop;
        Dictionary<string, int> metric;
        List<string> cur_lvl;
        readonly List<string> adreses;
        readonly Dictionary<string, List<string>> connections;
        public bool IsConnectedWithAll { get; private set; }

        public Router(string adr, List<string> adreses, Dictionary<string, List<string>> connections)
        {
            start_adr = adr;
            cur_lvl = new() { adr };
            this.adreses = adreses;
            this.connections = connections;

            next_hop = new();
            metric = new();
            foreach (var adres in adreses)
            {
                if (adres != adr)
                {
                    next_hop[adres] = "-----";
                    metric[adres] = -1;
                }
            }
            metric[adr] = 0;
            IsConnectedWithAll = false;
        }

        public void DoStep()
        {
            if (IsConnectedWithAll == true)
                return;

            List<string> new_lvl = new();
            foreach (var from in cur_lvl)
            {
                foreach (var to in connections[from])
                {
                    if (metric[to] == -1)
                    {
                        new_lvl.Add(to);
                        metric[to] = metric[from] + 1;
                        if (metric[to] > 1)
                            next_hop[to] = next_hop[from];
                        else
                            next_hop[to] = to;
                    }
                }
            }
            cur_lvl = new_lvl;

            if (cur_lvl.Count == 0)
                IsConnectedWithAll = true;
        }

        public void PrintTable(string text = null)
        {
            if (text == null)
                text = $"Table for {start_adr}:";

            Console.WriteLine(text);
            Console.WriteLine($"[Source IP]\t[Destination IP]\t[Next Hop]\t[Metric]");
            foreach (var adr in adreses)
            {
                if (adr != start_adr)
                {
                    Console.WriteLine($"{start_adr}\t{adr}\t\t{next_hop[adr]}\t\t{(metric[adr] > 0 ? metric[adr] : "\t-")}");
                }
            }
            Console.WriteLine();
        }
    }
}
