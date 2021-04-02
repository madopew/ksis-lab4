using System.Net.Sockets;

namespace Server.User
{
    public class GameUser : UserBase
    {
        public string Username { get; set; }

        public GameUser(string username, Socket handler)
            : base(handler)
        {
            Username = username;
        }

        public override string ToString()
        {
            return $"({(Username.Length == 0 ? "anonymous" : Username)}, {Handler.RemoteEndPoint})";
        }
    }
}