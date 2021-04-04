using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Client
{
    public sealed class ServerConnectionHandler
    {
        private readonly Dispatcher ui;
        
        private readonly Socket main;

        private Thread receiveThread;
        private bool isReceiving;

        private Action<string> lastMessageReceived;

        public ServerConnectionHandler(Dispatcher ui)
        {
            this.ui = ui;
            main = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public event Action<string> Received;
        
        public Task Connect(IPAddress address, int port)
        {
            return main.ConnectAsync(address, port);
        }

        public void Start()
        {
            isReceiving = true;
            receiveThread = new Thread(() =>
            {
                while (isReceiving)
                {
                    if (main.Available > 0)
                    {
                        var buffer = new byte[256];
                        main.Receive(buffer);
                        Receive(buffer);
                    }
                }
                
                Thread.Sleep(100);
            });
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        public void Send(string data, Action<string> onResponse)
        {
            if (main.Connected)
            {
                lastMessageReceived = onResponse;
                main.Send(Encoding.UTF8.GetBytes(data));
            }
        }

        private void Receive(byte[] data)
        {
            var content = Encoding.UTF8.GetString(data).TrimEnd('\0').TrimEnd('\n');
            if (content.Equals("ack")) return;
            if (content.Equals("success") || content.StartsWith("fail"))
            {
                if (lastMessageReceived != null)
                {
                    ui.Invoke(lastMessageReceived, content);
                    lastMessageReceived = null;
                }
            }
            else
            {
                if (Received != null)
                {
                    ui.Invoke(Received, content);
                }
            }
        }
    }
}