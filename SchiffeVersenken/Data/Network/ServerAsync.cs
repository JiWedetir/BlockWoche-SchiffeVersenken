using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SchiffeVersenken.Data.Network
{
    internal class ServerAsync
    {
        public bool _IsServerConnected => _client?.Connected ?? false;
        private TcpClient? _client = null;
        private TcpListener? _listener;
        public void StartServerAsync(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            Debug.WriteLine("Server gestartet, warte auf eine Verbindung...");

            Task.Run(async () => {
                while (NetworkConnection._IsRunning)
                {
                    if (_IsServerConnected)
                    {
                        _client = await _listener.AcceptTcpClientAsync();
                        NetworkConnection.ServerConnectedtoClient();
                        Debug.WriteLine("Client verbunden. Spiel kann beginnen...");
                        _ = HandleClientAsync(); // Startet eine neue Task, um den Client zu verarbeiten
                    }
                    else
                    {
                        // Wenn bereits ein Client verbunden ist, warten wir auf eine neue Verbindung
                        await Task.Delay(1000); // Wartezeit, bevor erneut auf eine Verbindung gewartet wird
                    }
                }
            });
        }


        private async Task HandleClientAsync()
        {
            var stream = _client.GetStream();
            var buffer = new byte[1024];

            try
            {
                while (NetworkConnection._IsRunning && _client.Connected)
                {
                    var count = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (count == 0) break;

                    var message = Encoding.UTF8.GetString(buffer, 0, count);
                    Console.WriteLine("Empfangen: " + message);

                    bool success = await NetworkConnection.ReceiveMessageAsync(message);
                    if (!success)
                    {
                        JObject? error = new JObject { { "Error", 1 } };
                        await SendMessageAsync(error.ToString());
                    }
                    
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                stream.Close();
                _client.Close();
            }
        }

        public async Task SendMessageAsync(string message)
        {
            var stream = _client.GetStream();
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }

        public void StopServer()
        {
            _client.Close();
            _listener.Stop();
            Debug.WriteLine("Server gestoppt.");
        }
    }
}
