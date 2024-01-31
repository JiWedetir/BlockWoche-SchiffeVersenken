using Newtonsoft.Json.Linq;
using SchiffeVersenken.Data.Model;
using System.Diagnostics;
using System.Globalization;

namespace SchiffeVersenken.Data.Network
{
    internal static class NetworkConnection
    {
        public static bool _IsRunning { get; private set; } 
        private static ServerAsync? _server;
        private static ClientAsync? _client;
        private static int _port = 5000; // muss noch geändert werden
        private static bool _isServer;
        private static GameLogic _game;
        private static List<(string message, string time)> _sentMessages = new List<(string message, string time)>();
        private static List<(string message, string time)> _receivedMessages = new List<(string message, string time)>();

        public static void StartNetwork()
        {
            _server = new ServerAsync();
            _server.StartServerAsync(_port);
        }

        public static void GameLogicConnectedtoNetwork(GameLogic gameLogic)
        {
            _game = gameLogic;
        }

        public static void ServerConnectedtoClient()
        {
            _isServer = true;
        }

        public static async Task<bool> ConnectToServer(string ip, string userName, int boardSize)
        {
            _client = new ClientAsync();
            await _client.ConnectAsync(ip, _port);
            SendInitMessage(userName, boardSize);
            _isServer = false;
            return _client._IsClientConnected;
        }

        public static async Task SendMessageAsync(string message)
        {
            if (_isServer)
            {
                await _server.SendMessageAsync(message);
            }
            else
            {
                await _client.SendMessageAsync(message);
            }
        }

        public static async Task<bool> ReceiveMessageAsync(string message)
        {
            bool valid = false;
            try
            {
                _receivedMessages.Add((message, DateTime.Now.ToString("d.M.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                var jObjectMessage = JObject.Parse(message);
                string type = jObjectMessage.Properties().FirstOrDefault()?.Name;
                switch (type)
                {
                    case "App":
                        valid = await ReciveInitMessage(jObjectMessage);
                        break;
                    case "Board":
                        valid = await ReciveBoardMessage(jObjectMessage);
                        break;
                    case "ShotAt":
                        valid = await ReciveShotAtMessage(jObjectMessage);
                        break;
                    case "Message":
                        valid = await ReciveTextMessage(jObjectMessage);
                        break;
                    case "Error":
                        valid = false;
                        break;
                    default:
                        valid = false;
                        break;
                }
                return valid;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private static async Task<bool> ReciveInitMessage(JObject message)
        {
            try
            {
                var app = message["App"].ToString();
                int.TryParse(message["Version"].ToString(), out int version);
                int.TryParse(message["BoardSize"].ToString(), out int boardSize);
                var userName = message["UserName"].ToString();
                // fragen ob gegen diesen spieler gespielt werden soll
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private static async Task<bool> ReciveBoardMessage(JObject message)
        {
            try
            {
                JArray boardArray = (JArray)message["Board"];
                List<JArray> rows = boardArray.ToObject<List<JArray>>();
                int[,] board = new int[rows.Count, rows[0].Count];

                for (int i = 0; i < rows.Count; i++)
                {
                    for (int j = 0; j < rows[i].Count; j++)
                    {
                        board[i, j] = (int)rows[i][j];
                    }
                }
                bool success = await _game._Opponent.SetShipAsync(board);
                return success;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private static async Task<bool> ReciveShotAtMessage(JObject message)
        {
            try
            {
                JObject shotAtObject = (JObject)message["ShotAt"];
                int.TryParse(shotAtObject["X"].ToString(), out int x);
                int.TryParse(shotAtObject["Y"].ToString(), out int y);
                _game.HandlePlayerInput(x, y);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private static async Task<bool> ReciveTextMessage(JObject message)
        {
            try
            {
                var messageText = message["Message"].ToString();
                // nachricht anzeigen im frontend
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private static async Task<bool> ReciveErrorMessage(JObject message)
        {
            try
            {
                string repetedMessage = _sentMessages[_sentMessages.Count - 1].message;
                SendMessageAsync(repetedMessage);
                _sentMessages.Add((repetedMessage, DateTime.Now.ToString("d.M.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private async static Task<bool> SendInitMessage(string userName, int boardSize)
        {
            try
            {
                JObject message = new JObject();
                message.Add("App", "SchiffeVersenkenG&T");
                message.Add("Version", 1);
                message.Add("BoardSize", boardSize);
                message.Add("UserName", userName);
                SendMessageAsync(message.ToString());
                _sentMessages.Add((message.ToString(), DateTime.Now.ToString("d.M.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async static Task<bool> SendBoardMessage(int[,] board)
        {
            try
            {
                JArray boardArray = new JArray();
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    JArray row = new JArray();
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        row.Add(board[i, j]);
                    }
                    boardArray.Add(row);
                }
                JObject message = new JObject();
                message.Add("Board", boardArray);
                SendMessageAsync(message.ToString());
                _sentMessages.Add((message.ToString(), DateTime.Now.ToString("d.M.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async static Task<bool> SendShotAtMessage(int x, int y)
        {
            try
            {
                JObject shotAtObject = new JObject
                {
                    { "X", x },
                    { "Y", y }
                };
                JObject message = new JObject
                {
                    { "ShotAt", shotAtObject }
                };
                SendMessageAsync(message.ToString());
                _sentMessages.Add((message.ToString(), DateTime.Now.ToString("d.M.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async static Task<bool> SendTextMessage(string messageText)
        {
            try
            {
                JObject message = new JObject();
                message.Add("Message", messageText);
                SendMessageAsync(message.ToString());
                _sentMessages.Add((message.ToString(), DateTime.Now.ToString("d.M.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}
