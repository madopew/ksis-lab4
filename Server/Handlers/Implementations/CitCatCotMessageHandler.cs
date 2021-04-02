using System;
using System.Collections.Generic;
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

        public void Handle(string content, GameUser user, GameServer server)
        {
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