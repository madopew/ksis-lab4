using System;
using System.Net;
using Server.Handlers;
using Server.Handlers.Builders;
using Server.Handlers.Implementations;
using Server.ServerBase;
using Server.ServerBase.Config;
using Server.ServerBase.Implementations;

namespace Server
{
    static class Program
    {
        private const string Ip = "192.168.100.3";
        private const ushort Port = 25565;
        static void Main(string[] args)
        {
            var messageHandler = new CCCMessageHandlerBuilder()
                .AddCommandHandler(new ConnectHandler())
                .AddCommandHandler(new DieHandler())
                .AddCommandHandler(new PlayHandler())
                .AddCommandHandler(new ListHandler())
                .AddCommandHandler(new MoveHandler())
                .AddFallbackHandler(new FallbackHandler())
                .Build();
            
            var server = new GameServer(new GameServerConfig
            {
                Address = IPAddress.Parse(Ip),
                AcceptQueueLength = 10,
                ListenDelay = 100,
                Port = Port,
                Handler = messageHandler,
            });

            server.ServerStarted += OnStart;
            server.UserConnected += OnUserConnected;
            server.UserMessaged += OnUserMessaged;
            server.UserDisconnected += OnUserDisconnected;
            
            server.Start();

            Console.ReadLine();
            
            server.Stop();
        }

        private static void OnStart(object sender, GameEventArgs args)
        {
            Console.WriteLine($"Server started at {Ip}:{Port}");
        }

        private static void OnUserConnected(object sender, GameEventArgs args)
        {
            Console.WriteLine($"Connected {args.User}");
        }
        
        private static void OnUserMessaged(object sender, GameEventArgs args)
        {
            Console.WriteLine($"{args.User} : {args.Command}");
        }

        private static void OnUserDisconnected(object sender, GameEventArgs args)
        {
            Console.WriteLine($"Disconnected: {args.User}");
        }
    }
}