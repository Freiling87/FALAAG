using System;
using Engine.EventArgs;

/* For info on why Singleton as opposed to Static, 
 * As well as general info on the MessageBroker pattern:
 * https://soscsrpg.com/build-a-c-wpf-rpg/lesson-16-1-adding-centralized-messaging/
 * Singleton aspects here:
 *  - Private constructor (to limit to 1 instance)
 *  - Private static variable to store Singleton
 *  - Static GetInstance() to retrieve Singleton
 */

namespace Engine.Services
{
    public class MessageBroker
    {
        private static readonly MessageBroker s_messageBroker = new MessageBroker();

        private MessageBroker() { }

        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        public static MessageBroker GetInstance() =>
            s_messageBroker;

        internal void RaiseMessage(string message) =>
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
    }
}