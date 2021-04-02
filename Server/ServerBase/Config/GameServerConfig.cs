using Server.Handlers.Interfaces;

namespace Server.ServerBase.Config
{
    public class GameServerConfig : ServerConfig
    {
        public IMessageHandler Handler { get; set; }
    }
}