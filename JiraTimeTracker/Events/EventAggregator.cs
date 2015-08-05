using System;
using System.Collections.Generic;

namespace Triosoft.JiraTimeTracker.Events
{
   public class EventAggregator
   {
      private readonly Dictionary<Type, List<Delegate>> _listeners = new Dictionary<Type, List<Delegate>>();

      public void Subscribe<T>(Action<T> handler)
      {
         Type eventType = typeof(T);

         if (!_listeners.ContainsKey(eventType))
         {
            _listeners.Add(eventType, new List<Delegate>());
         }

         List<Delegate> eventListeners = _listeners[eventType];
         eventListeners.Add(handler);
      }

      public void Raise<T>(T args)
      {
         Type eventType = typeof(T);

         if (_listeners.ContainsKey(eventType))
         {
            foreach (Delegate handler in _listeners[eventType])
            {
               ((Action<T>)handler)(args);
            }
         }
      }
   }
}