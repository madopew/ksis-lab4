using System.Text;
using Server.Handlers.Interfaces;
using Server.ServerBase.Implementations;
using Server.User;

namespace Server.Handlers.Implementations
{
    public class FallbackHandler : ICommandHandler
    {
        public bool Belongs(string message)
        {
            return true;
        }

        public void Handle(string message, GameUser user, GameServer server)
        {
            user.Handler.Send(Encoding.UTF8.GetBytes("fail/invalid syntax or command\n"));
        }
    }
}