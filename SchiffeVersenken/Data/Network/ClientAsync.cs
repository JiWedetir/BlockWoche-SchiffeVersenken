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

        /// <summary>
        /// Asynchronously connects to a specified IP address and port.
        /// </summary>
        /// <param name="ip">The IP address to connect to.</param>
        /// <param name="port">The port number to connect to.</param>
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

        /// <summary>
        /// Listens for incoming messages from the network and processes them asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to stop listening for messages.</param>
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

        /// <summary>
        /// Sends a message asynchronously.
        /// </summary>
        /// <param name="message">The message to send.</param>
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

        /// <summary>
        /// Disconnects the client from the server.
        /// </summary>
        public void Disconnect()
        {
            _cancellationTokenSource.Cancel();
            _stream.Close();
            _client.Close();
        }
    }
}
