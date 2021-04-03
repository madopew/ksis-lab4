using System.Linq;
using System.Text;
using Game;
using Server.Handlers.Interfaces;
using Server.ServerBase.Implementations;
using Server.User;

namespace Server.Handlers.Implementations
{
    public class PlayHandler : ICommandHandler
    {
        public bool Belongs(string message)
        {
            var inputs = message.Split('/');
            return inputs.Length == 2 && inputs[0].Equals("play");
        }

        public void Handle(string message, GameUser user, GameServer server)
        {
            var inputs = message.Split('/');
            if (user.Username.Length == 0)
            {
                user.Handler.Send(Encoding.UTF8.GetBytes("fail/not authorized\n"));
                return;
            }

            if (!server.Contains(inputs[1]))
            {
                user.Handler.Send(Encoding.UTF8.GetBytes("fail/second player not connected\n"));
                return;
            }

            if (user.Username.Equals(inputs[1]))
            {
                user.Handler.Send(Encoding.UTF8.GetBytes("fail/cannot play with himself\n"));
                return;
            }
            
            user.Challenged = server.GetUsers().First(p => p.Username.Equals(inputs[1]));
            if (user.Challenged?.Challenged?.Username.Equals(user.Username) == true)
            {
                user.Handler.Send(Encoding.UTF8.GetBytes($"start/{user.Challenged.Username}\n"));
                user.Challenged.Handler.Send(Encoding.UTF8.GetBytes($"start/{user.Username}\n"));

                var game = new CitCatCot(user.Challenged);
                user.Game = game;
                user.Challenged.Game = game;
                user.Opponent = user.Challenged;
                user.Opponent.Opponent = user;
                
                user.Challenged.Challenged = null;
                user.Challenged = null;
                return;
            }

            user.Challenged?.Handler.Send(Encoding.UTF8.GetBytes($"play/{user.Username}\n"));
        }
    }
}