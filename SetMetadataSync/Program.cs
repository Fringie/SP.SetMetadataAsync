using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetMetadataSync
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("{0} Starting app..", DateTime.Now);
            Engine e = new Engine();
            e.Start();
            Console.ReadKey();
        }

        
    }
}
