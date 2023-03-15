using System;
using System.Collections.Generic;
using System.Threading;

namespace EventThrottlingExample
{
    public class EventThrottler
    {
        private readonly Dictionary<string, List<Action>> _eventHandlers = new Dictionary<string, List<Action>>();
        private readonly Dictionary<string, Timer> _eventTimers = new Dictionary<string, Timer>();
        private readonly object _lockObject = new object();

        public void RegisterEventHandler(string eventName, Action eventHandler)
        {
            lock (_lockObject)
            {
                if (!_eventHandlers.ContainsKey(eventName))
                {
                    _eventHandlers[eventName] = new List<Action>();
                }
                _eventHandlers[eventName].Add(eventHandler);
            }
        }

        public void UnregisterEventHandler(string eventName, Action eventHandler)
        {
            lock (_lockObject)
            {
                if (_eventHandlers.ContainsKey(eventName))
                {
                    _eventHandlers[eventName].Remove(eventHandler);
                    if (_eventHandlers[eventName].Count == 0)
                    {
                        _eventHandlers.Remove(eventName);
                        if (_eventTimers.ContainsKey(eventName))
                        {
                            _eventTimers[eventName].Dispose();
                            _eventTimers.Remove(eventName);
                        }
                    }
                }
            }
        }

        public void ThrottleEvent(string eventName, int delayMilliseconds)
        {
            lock (_lockObject)
            {
                if (_eventHandlers.ContainsKey(eventName))
                {
                    if (!_eventTimers.ContainsKey(eventName))
                    {
                        _eventTimers[eventName] = new Timer((state) =>
                        {
                            var handlers = (List<Action>)state;
                            foreach (var handler in handlers)
                            {
                                handler();
                            }
                        }, _eventHandlers[eventName], delayMilliseconds, Timeout.Infinite);
                    }
                    else
                    {
                        _eventTimers[eventName].Change(delayMilliseconds, Timeout.Infinite);
                    }
                }
            }
        }

        public void TriggerEvent(string eventName)
        {
            lock (_lockObject)
            {
                if (_eventHandlers.ContainsKey(eventName))
                {
                    foreach (var handler in _eventHandlers[eventName])
                    {
                        handler();
                    }
                }
            }
        }
    }
}
