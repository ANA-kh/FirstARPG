using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State _currentState;
        protected virtual bool UseUnityUpdate { get; } = true;

        public void SwitchState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        private void Update()
        {
            if (UseUnityUpdate)
            {
                _currentState?.Tick(Time.deltaTime);
                OnUpdate();
            }
        }

        protected virtual void OnUpdate() { }
    }
}