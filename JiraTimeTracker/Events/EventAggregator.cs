using System;
using System.Collections.Generic;

namespace Triosoft.JiraTimeTracker.Events
{
   public class EventAggregator
   {
      private readonly Dictionary<Type, List<WeakReference>> _listeners = new Dictionary<Type, List<WeakReference>>();

      public void Subscribe<T>(Action<T> handler)
      {
         Type eventType = typeof(T);

         if (!_listeners.ContainsKey(eventType))
         {
            _listeners.Add(eventType, new List<WeakReference>());
         }

         List<WeakReference> eventListeners = _listeners[eventType];
         eventListeners.Add(new WeakReference(handler));
      }

      public void Raise<T>(T args)
      {
         Type eventType = typeof(T);

         if (_listeners.ContainsKey(eventType))
         {
            foreach (WeakReference handlerWeakReference in _listeners[eventType])
            {
               if (handlerWeakReference.Target != null)
               {
                  ((Action<T>)handlerWeakReference.Target)(args);
               }
            }
         }
      }
   }
}