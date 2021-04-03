using System;
using System.Text;
using Game;
using Server.Handlers.Interfaces;
using Server.ServerBase.Implementations;
using Server.User;

namespace Server.Handlers.Implementations
{
    public class MoveHandler : ICommandHandler
    {
        public bool Belongs(string message)
        {
            var inputs = message.Split('/');
            return inputs.Length == 3 && inputs[0].Equals("move");
        }

        public void Handle(string message, GameUser user, GameServer server)
        {
            if (user.Game is null)
            {
                user.Handler.Send(Encoding.UTF8.GetBytes("fail/not in the game\n"));
                return;
            }

            if (!user.Game.Moves(user))
            {
                user.Handler.Send(Encoding.UTF8.GetBytes("fail/not your turn\n"));
                return;
            }

            var inputs = message.Split('/');
            var i = Convert.ToInt32(inputs[1]);
            var j = Convert.ToInt32(inputs[2]);

            var stepResult = user.Game.Move(i, j);

            switch (stepResult)
            {
                case CitCatCotActionResult.Fail:
                    user.Handler.Send(Encoding.UTF8.GetBytes("fail/invalid move\n"));
                    break;
                case CitCatCotActionResult.Success:
                    user.Handler.Send(Encoding.UTF8.GetBytes("success\n"));
                    user.Opponent.Handler.Send(Encoding.UTF8.GetBytes($"game/{i}/{j}\n"));
                    break;
                case CitCatCotActionResult.Draw:
                    user.Handler.Send(Encoding.UTF8.GetBytes("game/draw\n"));
                    user.Opponent.Handler.Send(Encoding.UTF8.GetBytes($"game/{i}/{j}\ngame/draw\n"));
                    user.Opponent.Game = null;
                    user.Game = null;
                    break;
                case CitCatCotActionResult.WinFirst:
                {
                    var result = user.Game.First == user ? "win" : "lose";
                    var opponentResult = user.Game.First == user ? "lose" : "win";

                    user.Handler.Send(Encoding.UTF8.GetBytes($"game/{result}\n"));
                    user.Opponent.Handler.Send(Encoding.UTF8.GetBytes($"game/{i}/{j}\ngame/{opponentResult}\n"));
                    user.Opponent.Game = null;
                    user.Game = null;
                    break;
                }
                case CitCatCotActionResult.WinSecond:
                {
                    var opponentResult = user.Game.First == user ? "win" : "lose";
                    var result = user.Game.First == user ? "lose" : "win";

                    user.Handler.Send(Encoding.UTF8.GetBytes($"game/{result}\n"));
                    user.Opponent.Handler.Send(Encoding.UTF8.GetBytes($"game/{i}/{j}\ngame/{opponentResult}\n"));
                    user.Opponent.Game = null;
                    user.Game = null;
                    break;
                }
            }
        }
    }
}