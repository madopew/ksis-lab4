using System.Text;
using Server.Handlers.Interfaces;
using Server.ServerBase;
using Server.ServerBase.Implementations;
using Server.User;

namespace Server.Handlers.Implementations
{
    public class ConnectHandler : ICommandHandler
    {
        public bool Belongs(string message)
        {
            var inputs = message.Split('/');
            return inputs.Length == 2 && inputs[0].Equals("connect");
        }

        public void Handle(string message, GameUser user, GameServer server)
        {
            var inputs = message.Split('/');
            
            if (user.Username.Length != 0)
            {
                user.Handler.Send(Encoding.UTF8.GetBytes("fail/already connected\n"));
                return;
            }
            
            if (server.Contains(inputs[1]))
            {
                user.Handler.Send(Encoding.UTF8.GetBytes("fail/username exists\n"));
                return;
            }

            user.Username = inputs[1];
            user.Handler.Send(Encoding.UTF8.GetBytes("success\n"));
        }
    }
}