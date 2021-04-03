using Server.ServerBase.Implementations;
using Server.User;

namespace Server.Handlers.Interfaces
{
    public interface IMessageHandler
    {
        void Handle(byte[] data, GameUser user, GameServer server);
    }
}