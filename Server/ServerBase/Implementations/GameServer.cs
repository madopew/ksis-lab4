using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Server.Handlers.Interfaces;
using Server.ServerBase.Config;
using Server.ServerBase.Interfaces;
using Server.User;

namespace Server.ServerBase.Implementations
{
    public sealed class GameServer : IServer<GameUser>
    {
        private readonly IPAddress address;
        private readonly ushort port;
        private readonly int acceptQueueLength;
        private readonly int listenDelay;
        private readonly IMessageHandler handler;

        private readonly List<GameUser> pool;

        private Socket main;
        private Thread acceptThread;
        private bool isAccepting;
        private Thread receiveThread;
        private bool isReceiving;

        public GameServer(GameServerConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            address = config.Address ?? IPAddress.Any;
            port = config.Port == 0 ? (ushort)25565 : config.Port;
            acceptQueueLength = config.AcceptQueueLength == 0 ? 10 : config.AcceptQueueLength;
            listenDelay = config.ListenDelay == 0 ? 100 : config.ListenDelay;
            handler = config.Handler ?? throw new ArgumentNullException(nameof(config));

            pool = new List<GameUser>(16);
        }

        public event EventHandler<GameEventArgs> ServerStarted; 
        public event EventHandler<GameEventArgs> UserConnected; 
        public event EventHandler<GameEventArgs> UserDisconnected; 
        public event EventHandler<GameEventArgs> UserMessaged; 

        public void Start()
        {
            main = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            main.Bind(new IPEndPoint(address, port));
            main.Listen(acceptQueueLength);
            StartAccept();
            StartReceive();
            ServerStarted?.Invoke(this, new GameEventArgs(null, string.Empty));
        }

        public void Stop()
        {
            foreach (var user in pool)
            {
                try
                {
                    user.Handler.Shutdown(SocketShutdown.Both);
                    user.Handler.Close();
                }
                catch // dont care just closing
                {
                }
            }

            isAccepting = false;
            isReceiving = false;
            acceptThread.Join();
            receiveThread.Join();
        }

        public bool Contains(string username)
        {
            return pool.Exists(u => u.Username.Equals(username));
        }

        public void Kick(GameUser user)
        {
            user.Handler.Shutdown(SocketShutdown.Both);
            user.Handler.Close();
            UserDisconnected?.Invoke(this, new GameEventArgs(user, string.Empty));
            pool.Remove(user);
        }

        public IReadOnlyCollection<GameUser> GetUsers()
        {
            return new ReadOnlyCollection<GameUser>(pool);
        }

        private void StartAccept()
        {
            isAccepting = true;
            acceptThread = new Thread(() =>
            {
                while (isAccepting)
                {
                    var user = new GameUser(string.Empty, main.Accept());
                    pool.Add(user);
                    UserConnected?.Invoke(this, new GameEventArgs(user, string.Empty));
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
                    if (pool.Count != 0)
                    {
                        for(int i = pool.Count - 1; i >= 0; i--)
                        {
                            if (pool[i].Handler.Available > 0)
                            {
                                var data = new byte[256];
                                pool[i].Handler.Receive(data);

                                var content = Encoding.UTF8.GetString(data);
                                content = content.TrimEnd('\0');
                                content = content.TrimEnd('\n');
                                
                                UserMessaged?.Invoke(this, new GameEventArgs(pool[i], content));
                                handler.Handle(data, pool[i], this);
                            }
                        }
                    }

                    Thread.Sleep(listenDelay);
                }
            });
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
    }
}