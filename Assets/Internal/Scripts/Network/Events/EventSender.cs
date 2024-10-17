using System;
using System.Collections.Generic;
using Fusion;

namespace Shakhtarsk.Network.Events
{
    public abstract class EventSender<T> : EventSender
    {
        
        public override Type MessageType => typeof(T);

        protected T _invokedData;
        
        private readonly HashSet<Action<T>> _listeners = new ();

        public void Subscribe(Action<T> listener)
        {
            if (_invokedData == null) return;
            
            listener(_invokedData);
            _listeners.Add(listener);
        }

        public void Unsubscribe(Action<T> listener)
        {
            _listeners.Remove(listener);
        }
        
        protected void Invoke(T data)
        {
            _invokedData = data;
            foreach (var listener in _listeners)
            {
                listener(data);
            }
        }
    }
    
    public abstract class EventSender
    {
        public abstract Type MessageType { get; }
        
    }
}