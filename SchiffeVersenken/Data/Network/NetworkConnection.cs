using Newtonsoft.Json.Linq;
using SchiffeVersenken.Data.Database;
using SchiffeVersenken.Data.Model;
using System.Diagnostics;
using System.Globalization;

namespace SchiffeVersenken.Data.Network
{
    internal static class NetworkConnection
    {
        public static bool _IsRunning { get; private set; } 
        private static ServerAsync _server = new ServerAsync();
        private static ClientAsync _client = new ClientAsync();
        private static int _port = 5000; // muss noch geändert werden
        private static bool _isServer;
        private static GameLogic _game = new GameLogic();
        private static List<(string message, string time)> _sentMessages = new List<(string message, string time)>();
        private static List<(string message, string time)> _receivedMessages = new List<(string message, string time)>();

        /// <summary>
        /// Starts the network connection by creating a new server instance and starting it asynchronously.
        /// </summary>
        public static void StartNetwork()
        {
            _server.StartServerAsync(_port);
        }

        /// <summary>
        /// Sets the game logic instance for the network connection.
        /// </summary>
        /// <param name="gameLogic">The game logic instance to set.</param>
        public static void GameLogicConnectedtoNetwork(GameLogic gameLogic)
        {
            _game = gameLogic;
        }

        /// <summary>
        /// Sets the server flag to indicate that the server is connected to a client.
        /// </summary>
        public static void ServerConnectedtoClient()
        {
            _isServer = true;
        }

        /// <summary>
        /// Connects to the server with the specified IP address and board size.
        /// </summary>
        /// <param name="ip">The IP address of the server.</param>
        /// <param name="boardSize">The size of the game board.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the connection was successful.</returns>
        public static async Task<bool> ConnectToServer(string ip, int boardSize)
        {
            _client = new ClientAsync();
            await _client.ConnectAsync(ip, _port);
            await SendInitMessage(UserManagement._Player.Name, boardSize);
            _isServer = false;
            return _client._IsClientConnected;
        }

        /// <summary>
        /// Sends a message asynchronously.
        /// </summary>
        /// <param name="message">The message to send.</param>
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

        /// <summary>
        /// Receives a message asynchronously and processes it based on its type.
        /// </summary>
        /// <param name="message">The message to be received and processed.</param>
        /// <returns>A boolean value indicating whether the message was valid and processed successfully.</returns>
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
                        valid = ReciveInitMessage(jObjectMessage);
                        break;
                    case "Board":
                        valid = await ReciveBoardMessage(jObjectMessage);
                        break;
                    case "ShotAt":
                        valid = await ReciveShotAtMessage(jObjectMessage);
                        break;
                    case "Message":
                        valid = ReciveTextMessage(jObjectMessage);
                        break;
                    case "Error":
                        valid = await ReciveErrorMessage(jObjectMessage);
                        break;
                    default:
                        valid = false;
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                valid = false;
            }
            if (!valid)
            {
                await SendErrorMessage("Message not valide");
            }
            return valid;
        }

        /// <summary>
        /// Receives and processes the initialization message from the network.
        /// </summary>
        /// <param name="message">The initialization message received from the network.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a boolean value indicating whether the initialization message was successfully processed.</returns>
        private static bool ReciveInitMessage(JObject message)
        {
            try
            {
                var app = message["App"]?.ToString();
                int.TryParse(message["Version"]?.ToString(), out int version);
                int.TryParse(message["BoardSize"]?.ToString(), out int boardSize);
                var userName = message["UserName"]?.ToString();
                // fragen ob gegen diesen spieler gespielt werden soll
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Receives a board message and sets the ship positions on the opponent's board.
        /// </summary>
        /// <param name="message">The board message received.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the ship positions were set successfully.</returns>
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

        /// <summary>
        /// Receives a shot-at message from the opponent and handles the player input.
        /// </summary>
        /// <param name="message">The shot-at message received from the opponent.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the shot-at message was successfully processed.</returns>
        private static async Task<bool> ReciveShotAtMessage(JObject message)
        {
            if(_game._Opponent._YourTurn)
            {
                try
                {
                    JObject shotAtObject = (JObject)message["ShotAt"];
                    int.TryParse(shotAtObject["X"]?.ToString(), out int x);
                    int.TryParse(shotAtObject["Y"]?.ToString(), out int y);
                    _game.HandlePlayerInput(x, y);
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            else
            {
                await SendErrorMessage("Not your turn");
            }
            return false;
        }

        /// <summary>
        /// Receives a text message from the network and displays it in the frontend.
        /// </summary>
        /// <param name="message">The message received from the network.</param>
        /// <returns>True if the message was successfully received and displayed, false otherwise.</returns>
        private static bool ReciveTextMessage(JObject message)
        {
            try
            {
                var messageText = message["Message"]?.ToString();
                // show message in frontend
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Receives and handles error messages from the server.
        /// </summary>
        /// <param name="message">The error message received from the server.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the error message was successfully handled.</returns>
        private static async Task<bool> ReciveErrorMessage(JObject message)
        {
            try
            {
                switch (message["Error"]?.ToString())
                {
                    case "Message not valide":
                        string repetedMessage = _sentMessages[_sentMessages.Count - 1].message;
                        await SendMessageAsync(repetedMessage);
                        _sentMessages.Add((repetedMessage, DateTime.Now.ToString("d.M.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                        break;
                    case "Not your turn":
                        // nachricht anzeigen im frontend
                        break;
                    default:
                        return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Sends an initialization message to the server with the specified user name and board size.
        /// </summary>
        /// <param name="userName">The user name to send.</param>
        /// <param name="boardSize">The board size to send.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the message was sent successfully.</returns>
        private async static Task<bool> SendInitMessage(string userName, int boardSize)
        {
            try
            {
                JObject message = new JObject();
                message.Add("App", "SchiffeVersenkenG&T");
                message.Add("Version", 1);
                message.Add("BoardSize", boardSize);
                message.Add("UserName", userName);
                await SendMessageAsync(message.ToString());
                _sentMessages.Add((message.ToString(), DateTime.Now.ToString("d.M.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Sends the game board as a message to the network.
        /// </summary>
        /// <param name="board">The game board represented as a 2D array.</param>
        /// <returns>A task representing the asynchronous operation. Returns true if the message was sent successfully, false otherwise.</returns>
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
                await SendMessageAsync(message.ToString());
                _sentMessages.Add((message.ToString(), DateTime.Now.ToString("d.M.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Sends a shot at message to the network with the specified coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the shot.</param>
        /// <param name="y">The y-coordinate of the shot.</param>
        /// <returns>A task representing the asynchronous operation. The task result is true if the message was sent successfully; otherwise, false.</returns>
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
                await SendMessageAsync(message.ToString());
                _sentMessages.Add((message.ToString(), DateTime.Now.ToString("d.M.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Sends a text message over the network connection.
        /// </summary>
        /// <param name="messageText">The text of the message to send.</param>
        /// <returns>A task representing the asynchronous operation. The task result is true if the message was sent successfully; otherwise, false.</returns>
        public async static Task<bool> SendTextMessage(string messageText)
        {
            try
            {
                JObject message = new JObject
                {
                    { "Message", messageText }
                };
                await SendMessageAsync(message.ToString());
                _sentMessages.Add((message.ToString(), DateTime.Now.ToString("d.M.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Sends an error message to the network connection.
        /// </summary>
        /// <param name="error">The error message to send.</param>
        /// <returns>A task representing the asynchronous operation. Returns true if the error message was sent successfully; otherwise, false.</returns>
        public async static Task<bool> SendErrorMessage(string error)
        {
            try
            {
                JObject message = new JObject
                {
                    { "Error", error }
                };
                await SendMessageAsync(message.ToString());
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
