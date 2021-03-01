using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class ClientEntity
    {
        public string Id { get; private set; }
        string _username;
        ServerEntity _server;
        TcpClient _client;
        NetworkStream _stream;

        public ClientEntity(TcpClient client, ServerEntity server)
        {
            Id = Guid.NewGuid().ToString();
            _client = client;
            _server = server;
        }

        public void Processing()
        {
            try
            {
                _stream = _client.GetStream();
                string message = DecodeMessage();

                _username = message;

                message += " enter to the chat!";
                _server.Broadcast(message, Id);
                Console.WriteLine(message);

                while (true)
                {
                    try
                    {
                        message = DecodeMessage();
                        _server.Broadcast($"{_username}: {message}", Id);
                        Console.WriteLine(message);                        
                    }
                    catch (Exception ex)
                    {
                        message = $"{_username} left the chat (((";
                        _server.Broadcast(message, Id);
                        Console.WriteLine(message);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"ERROR: {ex.Message}");
            }
            finally
            {
                Disconnect();
                _server.DisconnectClient(Id);
            }
        }

        private string DecodeMessage()
        {
            byte[] buf = new byte[512];
            int bytesCount = 0;
            string message = string.Empty;
            do
            {
                bytesCount = _stream.Read(buf, 0, buf.Length);
                message += Encoding.UTF8.GetString(buf, 0, bytesCount);
            } while (_stream.DataAvailable);

            return message;
        }

        public void SendMessage(byte[] data)
        {
            _stream.Write(data, 0, data.Length);
        }

        public void Disconnect()
        {
            _stream?.Close();
            _client?.Close();
        }
    }
}
