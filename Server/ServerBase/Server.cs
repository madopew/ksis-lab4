/*using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Server.User;

namespace Server.ServerBase
{
    public partial class Server<T> where T : UserBase
    {
        private const int MaxQueueLength = 10;
        private const int Delay = 100;
        
        private Socket main;
        
        private readonly IPAddress address;
        private readonly ushort port;

        private readonly List<T> playersPool;
        
        private Thread acceptThread;
        private bool isAccepting;

        private Thread receiveThread;
        private bool isReceiving;

        public event EventHandler<GameEventArgs> UserConnected;
        public event EventHandler<GameEventArgs> UserDisconnected; 

        public void Start()
        {
            main = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            main.Bind(new IPEndPoint(address, port));
            main.Listen(MaxQueueLength);
            StartAccept();
            StartReceive();
        }

        public void Stop()
        {
                
            isAccepting = false;
            isReceiving = false;
            acceptThread.Join();
            receiveThread.Join();
        }

        private void StartAccept()
        {
            isAccepting = true;
            acceptThread = new Thread(() =>
            {
                while (isAccepting)
                {
                    playersPool.Add(new User(string.Empty, main.Accept()));
                }
            });
            acceptThread.IsBackground = true;
            acceptThread.Start();
        }

        private void StartReceive()
        {
            isReceiving = true;
            receiveThread = new Thread(() =>
            {
                while (isReceiving)
                {
                    if (playersPool.Count != 0)
                    {
                        for(int i = playersPool.Count - 1; i >= 0; i--)
                        {
                            if (playersPool[i].Handler.Available > 0)
                            {
                                var data = new byte[256];
                                playersPool[i].Handler.Receive(data);
                                HandleMessage(data, playersPool[i]);
                            }
                        }
                    }

                    Thread.Sleep(Delay);
                }
            });
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        private void HandleMessage(byte[] data, User player)
        {
            var message = Encoding.UTF8.GetString(data);
            message = message.TrimEnd('\0');
            if (message[^1] != '\n')
            {
                player.Handler.Send(Encoding.UTF8.GetBytes("fail/invalid syntax\n"));
                return;
            }

            message = message[..^1];

            var input = message.Split('/');

            if (input.Length < 2)
            {
                player.Handler.Send(Encoding.UTF8.GetBytes("fail/invalid syntax\n"));
                return;
            }

            var username = input[0];
            switch (input[1])
            {
                case "connect":
                    HandleConnect(username, player);
                    break;
                case "die":
                    HandleDisconnect(player);
                    break;
                case "play":
                    if (input.Length != 3)
                    {
                        player.Handler.Send(Encoding.UTF8.GetBytes("fail/no second user\n"));
                        break;
                    }

                    HandlePlay(player, input[2]);
                    break;
                default:
                    Console.WriteLine($"invalid: {message}");
                    player.Handler.Send(Encoding.UTF8.GetBytes("fail/unknown command\n"));
                    break;
            }
        }

        private void HandleConnect(string username, User player)
        {
            if (player.Username.Length != 0)
            {
                player.Handler.Send(Encoding.UTF8.GetBytes("fail/already connected\n"));
                return;
            }

            if (IsConnected(username))
            {
                player.Handler.Send(Encoding.UTF8.GetBytes("fail/username exists\n"));
                return;
            }

            UserConnected?.Invoke(this, new GameEventArgs {Username = username});
            player.Username = username;
            player.Handler.Send(Encoding.UTF8.GetBytes("success\n"));
        }

        private bool IsConnected(string username)
        {
            return username.Length != 0 && playersPool.Exists(p => p.Username.Equals(username));
        }

        private void HandleDisconnect(User player)
        {
            player.Handler.Shutdown(SocketShutdown.Both);
            player.Handler.Close();
            player.Handler.Dispose();
            UserDisconnected?.Invoke(this, new GameEventArgs {Username = player.Username});
            playersPool.Remove(player);
        }

        private void HandlePlay(User player, string second)
        {
            if (!IsConnected(player.Username))
            {
                player.Handler.Send(Encoding.UTF8.GetBytes("fail/not connected\n"));
                return;
            }

            if (!IsConnected(second))
            {
                player.Handler.Send(Encoding.UTF8.GetBytes("fail/second player not connected\n"));
                return;
            }

            if (player.Username.Equals(second))
            {
                player.Handler.Send(Encoding.UTF8.GetBytes("fail/cannot play with himself\n"));
                return;
            }
            
            player.Challenged = playersPool.Find(p => p.Username.Equals(second));
            if (player.Challenged?.Challenged?.Username.Equals(player.Username) == true)
            {
                player.Handler.Send(Encoding.UTF8.GetBytes($"start/{player.Challenged.Username}\n"));
                player.Challenged.Handler.Send(Encoding.UTF8.GetBytes($"start/{player.Username}\n"));
                // TODO start a game
                player.Challenged.Challenged = null;
                player.Challenged = null;
                return;
            }

            player.Challenged?.Handler.Send(Encoding.UTF8.GetBytes($"play/{player.Username}\n"));
        }
    }
}*/