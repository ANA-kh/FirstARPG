using System.Collections.Generic;

namespace MEventSystem.EventSystemByID
{
    public delegate void EventHandler(params object[] data);
    
    public interface IIdEventSystem
    {
        public void Send(uint eventID, params object[] data);
        public void Register(uint eventID, EventHandler eventHandler);
        public void UnRegister(uint eventID, EventHandler eventHandler);
    }
    
    
    public class IdEventSystem : IIdEventSystem
    {
        public static IIdEventSystem EventSystem = new IdEventSystem();
        private Dictionary<uint, EventHandler> _handlersDic;

        public IdEventSystem()
        {
            _handlersDic = new Dictionary<uint, EventHandler>();
        }
        public void Send(uint eventID, params object[] data)
        {
            if (_handlersDic.TryGetValue(eventID, out var handler))
            {
                handler(data);
            }
        }

        public void Register(uint eventID, EventHandler eventHandler)
        {
            if (_handlersDic.TryGetValue(eventID, out var handler))
            {
                _handlersDic[eventID] = handler + eventHandler;
            }
            else
            {
                _handlersDic[eventID] = eventHandler;
            }
        }

        public void UnRegister(uint eventID, EventHandler eventHandler)
        {
            if (_handlersDic.TryGetValue(eventID, out var handler))
            {
                var newHandler = handler - eventHandler;
                if (newHandler == null)
                {
                    _handlersDic.Remove(eventID);
                }
                else
                {
                    _handlersDic[eventID] = newHandler;
                }
            }
        }
    }
}