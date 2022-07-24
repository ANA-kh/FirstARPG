using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.StateMachine
{
    
    public abstract class StateMachine : MonoBehaviour
    {
        private State _currentState;

        public void SwitchState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        private void Update()
        {
            _currentState?.Tick(Time.deltaTime);
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
        }
    }
}