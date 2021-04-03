using System;
using System.Collections.Generic;
using System.Text;
using Server.Handlers.Interfaces;
using Server.ServerBase.Implementations;
using Server.User;

namespace Server.Handlers.Implementations
{
    public class CitCatCotMessageHandler : IMessageHandler
    {
        private readonly ICollection<ICommandHandler> handlers;
        private readonly ICommandHandler fallback;
        internal CitCatCotMessageHandler(ICollection<ICommandHandler> handlers, ICommandHandler fallback)
        {
            this.handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            this.fallback = fallback ?? throw new ArgumentNullException(nameof(fallback));
        }

        public void Handle(byte[] data, GameUser user, GameServer server)
        {
            string content = Encoding.UTF8.GetString(data).TrimEnd('\0').TrimEnd('\n');
            foreach (var handler in handlers)
            {
                if (handler.Belongs(content))
                {
                    handler.Handle(content, user, server);
                    return;
                }
            }

            fallback.Handle(content, user, server);
        }
    }
}