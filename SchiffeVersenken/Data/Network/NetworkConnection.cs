using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SchiffeVersenken.Data.Network
{
    internal static class NetworkConnection
    {
        public static bool IsRunning { get; private set; } 
        private static ServerAsync? server;
        private static ClientAsync? client;
        private static int port = 5000; // muss noch geändert werden
        private static bool isServer;

        public static void StartNetwork()
        {
            server = new ServerAsync();
            server.StartServerAsync(port);
        }

        public static void ServerConnectedtoClient()
        {
            isServer = true;
        }

        public static async Task<bool> ConnectToServer(string ip)
        {
            client = new ClientAsync();
            await client.ConnectAsync(ip, port);
            isServer = false;
            return client.IsClientConnected;
        }

        public static async Task SendMessageAsync(string message)
        {
            if (isServer)
            {
                await server.SendMessageAsync(message);
            }
            else
            {
                await client.SendMessageAsync(message);
            }
        }

        public static async void ReceiveMessageAsync(string message)
        {
            switch
        }
    }
}
