﻿using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SchiffeVersenken.Data.Network
{
    internal class ServerAsync
    {
        public bool IsServerConnected => client.Connected;
        private TcpClient client;
        private TcpListener listener;
        public void StartServerAsync(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Debug.WriteLine("Server gestartet, warte auf eine Verbindung...");

            Task.Run(async () => {
                while (NetworkConnection.IsRunning)
                {
                    if (IsServerConnected)
                    {
                        client = await listener.AcceptTcpClientAsync();
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
            var stream = client.GetStream();
            var buffer = new byte[1024];

            try
            {
                while (NetworkConnection.IsRunning && client.Connected)
                {
                    var count = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (count == 0) break;

                    var message = Encoding.UTF8.GetString(buffer, 0, count);
                    Console.WriteLine("Empfangen: " + message);

                    
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }

        public async Task SendMessageAsync(string message)
        {
            var stream = client.GetStream();
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }

        public void StopServer()
        {
            client.Close();
            listener.Stop();
            Debug.WriteLine("Server gestoppt.");
        }
    }
}
