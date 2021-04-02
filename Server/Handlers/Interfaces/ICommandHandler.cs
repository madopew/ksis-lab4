using Server.ServerBase.Implementations;
using Server.User;

namespace Server.Handlers.Interfaces
{
    public interface ICommandHandler
    {
        bool Belongs(string message);
        void Handle(string message, GameUser user, GameServer server);
    }
}