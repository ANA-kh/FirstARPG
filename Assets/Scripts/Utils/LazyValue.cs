using System;
using UnityEngine;

namespace FirstARPG.Utils
{
    /// <summary>
    /// 容器类
    /// 使T在第一次调用前初始化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyValue<T>
    {
        private T _value;
        private bool _initialized = false;
        private InitializerDelegate _initializer;

        public delegate T InitializerDelegate();

        /// <summary>
        /// 构造容器，设置初始化方法
        /// </summary>
        /// <param name="initializer">初始化方法</param>
        public LazyValue(InitializerDelegate initializer)
        {
            _initializer = initializer;
        }

        
        public T value
        {
            get
            {
                // 确保使用前初始化
                ForceInit();
                return _value;
            }
            set
            {
                _initialized = true;
                _value = value;
            }
        }

        public void ForceInit()
        {
            if (!_initialized)
            {
                _value = _initializer();
                _initialized = true;
            }
        }
    }
}