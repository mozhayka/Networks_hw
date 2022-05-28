using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIP
{
    class Network
    {
        readonly List<string> adreses;
        readonly Dictionary<string, List<string>> connections;
        List<Router> routers;

        public Network(int v = 5, int e = 5)
        {
            var ng = new NetworkGenerator(v, e);
            adreses = ng.adreses;
            connections = ng.connections;
            routers = new();
            foreach (var adr in adreses)
            {
                routers.Add(new Router(adr, adreses, connections));
            }
        }

        public void Run()
        {
            bool stop = false;
            int step = 1;
            int n = routers.Count;
            Task[] tasks = new Task[n];

            while (!stop)
            {
                bool IsAllConnected = true;

                for (int i = 0; i < n; i++)
                {
                    int j = i;
                    tasks[j] = new Task(() => routers[j].DoStep());
                }

                foreach (var task in tasks)
                    task.Start();

                Task.WaitAll(tasks);
                foreach (var router in routers)
                {
                    router.PrintTable($"Table at step {step} for {router.start_adr}:");
                    IsAllConnected = IsAllConnected && router.IsConnectedWithAll;
                }
                stop = IsAllConnected;
                step++;
            }

            foreach (var router in routers)
            {
                router.PrintTable($"Final table for {router.start_adr}:");
            }
        }
    }
}
