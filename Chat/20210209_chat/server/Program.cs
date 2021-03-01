using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerEntity server = null;
            try
            {
                server = new ServerEntity("127.0.0.1", 8888);
                new Thread(server.Listen).Start();
            }
            catch (Exception ex)
            {
                server?.DisconnectAll();
                //Console.WriteLine($"ERROR: {ex.Message}");                
            }
        }
    }
}
