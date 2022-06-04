using System;
using System.Collections.Generic;

namespace EventSystem.EventSystemByType
{
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        IUnRegister Register<T>(Action<T> onEvent); //返回注销对象，避免忘记注销
        void UnRegister<T>(Action<T> onEvent);
    }

    public interface IUnRegister
    {
        void UnRegister();
    }
    
    public struct TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem TypeEventSystem;
        public Action<T> OnEvent;
        public void UnRegister()
        {
            TypeEventSystem.UnRegister<T>(OnEvent);
            TypeEventSystem = null;
            OnEvent = null;
        }
    }
    
    public class TypeEventSystem : ITypeEventSystem
    {
        //通过包裹一层接口，延迟T的赋值，实现在字典中存储不同的Action<T>  （T不同）
        private Dictionary<Type, ITypeEventHandler> _eventHandlers;
        public interface ITypeEventHandler
        {
            
        }
        
        public class TypeEventHandler<T> : ITypeEventHandler
        {
            public Action<T> handler;
        }

        public TypeEventSystem()
        {
            _eventHandlers = new Dictionary<Type, ITypeEventHandler>();
        }
        public void Send<T>() where T : new()
        {
            var e= new T();
            Send<T>(e);
        }

        public void Send<T>(T e)
        {
            var type = typeof(T);
            if (_eventHandlers.TryGetValue(type, out var handler))
            {
                (handler as TypeEventHandler<T>).handler(e);
            }
        }

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            ITypeEventHandler handler;
            if (!_eventHandlers.TryGetValue(type, out handler))
            {
                handler = new TypeEventHandler<T>();
                _eventHandlers.Add(type,handler);
            }
            (handler as TypeEventHandler<T>).handler += onEvent;
            
            return new TypeEventSystemUnRegister<T>()
            {
                TypeEventSystem = this,
                OnEvent = onEvent
            };
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            if (_eventHandlers.TryGetValue(type, out var handler))
            {
                var typeHandler = (handler as TypeEventHandler<T>);
                typeHandler.handler -= onEvent;
                
                //handler减掉所有注册函数后会变成null，删除索引否则会空指针
                //或者调用时使用handler?.Invoke(e)替换handler(e)
                if (typeHandler.handler == null)
                {
                    _eventHandlers.Remove(type);
                }
            }
        }
    }
}