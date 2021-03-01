using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class Program
    {
        static int port = 9999;
        static string localIp = "127.0.0.1";

        static void Main(string[] args)
        {
            IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Parse(localIp), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {

                socket.Bind(localEndpoint);
                socket.Listen(15);

                Console.WriteLine($"Server started...  {localIp}:{port}");

                while (true)
                {
                    Socket remoteSocket = socket.Accept();

                    byte[] buffer = new byte[1024];
                    int bytesCount = 0;
                    string message = String.Empty;

                    do
                    {
                        bytesCount = remoteSocket.Receive(buffer);
                        message += Encoding.UTF8.GetString(buffer, 0, bytesCount);
                    } while (remoteSocket.Available > 0);

                    Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {message}");

                    string response = "message received...";
                    remoteSocket.Send(Encoding.UTF8.GetBytes(response));

                    remoteSocket.Shutdown(SocketShutdown.Both);
                    remoteSocket.Close();

                    if(message.ToLower() == "shutdown")
                        Process.Start("cmd", "/c shutdown -s -t 00");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}
