using System.Net;
using Server.Handlers.Interfaces;

namespace Server.ServerBase.Config
{
    public class ServerConfig
    {
        public IPAddress Address { get; set; }
        public ushort Port { get; set; }
        public int AcceptQueueLength { get; set; }
        public int ListenDelay { get; set; }
    }
}