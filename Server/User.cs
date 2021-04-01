using System.Net.Sockets;

namespace Server
{
    public partial class Server
    {
        private class User
        {
            public string Username { get; set; }
            public Socket Handler { get; }
            
            public User Challenged { get; set; }

            public User(string username, Socket socket)
            {
                Username = username;
                Handler = socket;
            }
        }
    }
}