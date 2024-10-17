using System;
using System.Collections.Generic;
using Fusion;

namespace Shakhtarsk.Network.Events
{
    public class NetworkedEvents
    {
        private readonly Dictionary<Type, EventSender> _eventMap = new ();
        
        public NetworkedEvents(NetworkEvents events)
        {
            Register<SessionListEventSender, SessionList>(events);
            Register<NetworkInputEventSender, NetworkPlayerInputArgs>(events);
            Register<PlayerLeftSender, PlayerLeftEvent>(events);
        }

        public EventSender<T> GetSender<T>() 
        {
            var type = typeof(T);
            
            return _eventMap[type] as EventSender<T>;
        }

        private void Register<T, TM>(NetworkEvents events) where T : NetworkedEventSender<TM>, new()
        {
            var sender = new T();
            sender.Init(events);
            
            _eventMap[sender.MessageType] = sender;
        }
    }

    public class NetworkedEventSender<T> : EventSender<T>
    {
        public virtual void Init(NetworkEvents events){ }
    }
}