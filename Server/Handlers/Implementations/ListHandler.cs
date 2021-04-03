using System.Linq;
using System.Text;
using Server.Handlers.Interfaces;
using Server.ServerBase.Implementations;
using Server.User;

namespace Server.Handlers.Implementations
{
    public class ListHandler : ICommandHandler
    {
        public bool Belongs(string message)
        {
            return message.Equals("list");
        }

        public void Handle(string message, GameUser user, GameServer server)
        {
            if (user.Username.Length == 0)
            {
                user.Handler.Send(Encoding.UTF8.GetBytes("fail/not authorized\n"));
                return;
            }

            var users = server.GetUsers();
            var result = new string[users.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = users.ElementAt(i).Username;
            }

            user.Handler.Send(Encoding.UTF8.GetBytes($"{string.Join("/", result)}\n"));
        }
    }
}