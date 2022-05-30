using System;

namespace Tracert
{
    class Program
    {
        static void Main(string[] args)
        {
            TraceRoute tr = new("yandex.ru");
            tr.Run();
        }
    }
}
