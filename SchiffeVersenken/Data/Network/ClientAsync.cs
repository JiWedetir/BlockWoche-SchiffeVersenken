using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace SchiffeVersenken.Data.Network
{
    internal class ClientAsync
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private CancellationTokenSource _cancellationTokenSource;
        public bool _IsClientConnected => _client.Connected;
        public async Task ConnectAsync(string ip, int port)
        {
            try
            {
                _client = new TcpClient();
                _cancellationTokenSource = new CancellationTokenSource();
                await _client.ConnectAsync(ip, port);
                _stream = _client.GetStream();
            
                Task.Run(() => ListenForMessage(_cancellationTokenSource.Token));
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message);
            }
        }

        private async Task ListenForMessage(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var buffer = new byte[1024];
                    var count = await _stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    if (count == 0) break;

                    var message = Encoding.UTF8.GetString(buffer, 0, count);

                    bool success = await NetworkConnection.ReceiveMessageAsync(message);
                    if (!success)
                    {
                        JObject error = new JObject { { "Error", 1 } };
                        await SendMessageAsync(error.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message);
            }
        }

        public async Task SendMessageAsync(string message)
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await _stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message);
            }
        }

        public void Disconnect()
        {
            _cancellationTokenSource.Cancel();
            _stream.Close();
            _client.Close();
        }
    }
}
