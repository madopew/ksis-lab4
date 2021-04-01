using System;
using System.Net;

namespace Server
{
    static class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(IPAddress.Any, 25565);
            server.UserConnected += OnUserConnected;
            server.UserDisconnected += OnUserDisconnected;
            server.Start();

            Console.WriteLine($"Server started ad {IPAddress.Any}:25565");
            Console.ReadLine();
        }

        private static void OnUserConnected(object sender, GameEventArgs args)
        {
            Console.WriteLine($"Connected {args.Username}");
        }

        private static void OnUserDisconnected(object sender, GameEventArgs args)
        {
            Console.WriteLine($"Disconnected: {args.Username}");
        }
    }
}