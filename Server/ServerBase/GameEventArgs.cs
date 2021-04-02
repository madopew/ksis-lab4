using System;
using Server.User;

namespace Server.ServerBase
{
    public class GameEventArgs : EventArgs
    {
        public GameUser User { get; set; }
        public string Command { get; set; }

        public GameEventArgs(GameUser user, string command)
        {
            User = user;
            Command = command;
        }
    }
}