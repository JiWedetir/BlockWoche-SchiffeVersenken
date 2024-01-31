using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

namespace SchiffeVersenken.Data.Network
{
    internal class ClientAsync
    {
        private TcpClient client;
        private NetworkStream stream;
        private CancellationTokenSource cancellationTokenSource;
        public bool IsClientConnected => client.Connected;
        public async Task ConnectAsync(string ip, int port)
        {
            try
            {
                client = new TcpClient();
                cancellationTokenSource = new CancellationTokenSource();
                await client.ConnectAsync(ip, port);
                stream = client.GetStream();
            
                Task.Run(() => ListenForMessage(cancellationTokenSource.Token));
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
                    var count = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    if (count == 0) break;

                    var message = Encoding.UTF8.GetString(buffer, 0, count);
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
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message);
            }
        }

        public void Disconnect()
        {
            cancellationTokenSource.Cancel();
            stream.Close();
            client.Close();
        }
    }
}
