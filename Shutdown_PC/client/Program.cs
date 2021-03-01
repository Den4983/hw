using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        static int port = 9999;
        static string ip = "127.0.0.1";
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);                

                while (true)
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(remoteEndpoint);

                    Console.Write(">>> ");
                    string message = Console.ReadLine();
                    socket.Send(Encoding.UTF8.GetBytes(message));

                    byte[] buffer = new byte[1024];
                    int bytesCount = 0;
                    string response = String.Empty;

                    do
                    {
                        bytesCount = socket.Receive(buffer);
                        response += Encoding.UTF8.GetString(buffer, 0, bytesCount);
                    } while (socket.Available > 0);

                    Console.WriteLine($"Server say: {response}");
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                    if (message == "quit")
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}
