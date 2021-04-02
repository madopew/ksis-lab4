using Server.ServerBase.Implementations;
using Server.User;

namespace Server.Handlers.Interfaces
{
    public interface IMessageHandler
    {
        void Handle(string data, GameUser user, GameServer server);
    }
}