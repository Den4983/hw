using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        static string _userName;
        static string _host = "127.0.0.1";
        static int _port = 8888;
        static TcpClient _client;
        static NetworkStream _stream;

        static void Main(string[] args)
        {
            Console.Write("Enter your username: ");
            _userName = Console.ReadLine();

            try
            {
                _client = new TcpClient();

                _client.Connect(_host, _port);
                _stream = _client.GetStream();

                byte[] data = Encoding.UTF8.GetBytes(_userName);
                _stream.Write(data, 0, data.Length);

                new Thread(ReceiveMessages).Start();

                SendMessages();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            finally
            {
                _stream?.Close();
                _client?.Close();
            }
        }

        static void ReceiveMessages()
        {
            try
            {
                while (true)
                {
                    byte[] buf = new byte[1024];
                    int byteCount = 0;
                    string message = string.Empty;

                    do
                    {
                        byteCount = _stream.Read(buf, 0, buf.Length);
                        message += Encoding.UTF8.GetString(buf, 0, byteCount);
                    } while (_stream.DataAvailable);

                    Console.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            finally
            {
                _stream?.Close();
                _client?.Close();
            }
        }

        static void SendMessages()
        {
            while (true)
            {
                Console.Write(">>>");
                string message = Console.ReadLine();

                byte[] data = Encoding.UTF8.GetBytes(message);
                _stream.Write(data, 0, data.Length);
            }
        }
    }
}
