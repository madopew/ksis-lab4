using System.Net.Sockets;

namespace Server.User
{
    public abstract class UserBase
    {
        public Socket Handler { get; }

        public UserBase(Socket handler)
        {
            Handler = handler;
        }
    }
}