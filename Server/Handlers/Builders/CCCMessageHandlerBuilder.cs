using System;
using System.Collections.Generic;
using Server.Handlers.Implementations;
using Server.Handlers.Interfaces;
using Server.ServerBase.Implementations;
using Server.User;

namespace Server.Handlers.Builders
{
    public class CCCMessageHandlerBuilder
    {
        private List<ICommandHandler> commands;
        private ICommandHandler fallback;

        public CCCMessageHandlerBuilder()
        {
            commands = new List<ICommandHandler>();
        }

        public CitCatCotMessageHandler Build()
        {
            if (fallback is null)
            {
                throw new InvalidOperationException("no fallback handler");
            }

            return new CitCatCotMessageHandler(commands, fallback);
        }

        public CCCMessageHandlerBuilder AddCommandHandler(ICommandHandler handler)
        {
            commands.Add(handler);
            return this;
        }

        public CCCMessageHandlerBuilder AddFallbackHandler(ICommandHandler handler)
        {
            fallback = handler;
            return this;
        }
    }
}