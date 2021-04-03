using System.Net;
using System.Net.Sockets;
using Game;

namespace Server.User
{
    public class GameUser : UserBase
    {
        public string Username { get; set; }
        public GameUser Challenged { get; set; }
        public GameUser Opponent { get; set; }
        public CitCatCot Game { get; set; }

        public GameUser(string username, Socket handler)
            : base(handler)
        {
            Username = username;
        }

        public override string ToString()
        {
            EndPoint endPoint;
            try
            {
                endPoint = Handler.RemoteEndPoint;
            }
            catch // just for output
            {
                endPoint = null;
            }

            return $"({(Username.Length == 0 ? "anonymous" : Username)}, {(endPoint is null ? "closed" : endPoint.ToString())})";
        }
    }
}