using System.Net.Sockets;
using Server.Handlers.Interfaces;
using Server.ServerBase.Implementations;
using Server.User;

namespace Server.Handlers.Implementations
{
    public class DieHandler : ICommandHandler
    {
        public bool Belongs(string message)
        {
            return message.Equals("die");
        }

        public void Handle(string message, GameUser user, GameServer server)
        {
            server.Kick(user);
        }
    }
}