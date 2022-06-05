using System;
using System.Collections.Generic;
using System.Linq;

namespace FSM
{
    public sealed class Fsm<T> : IFsm<T> where T : class
    {
        private string _name;
        private T _owner;
        private readonly Dictionary<Type, FsmState<T>> _states;
        private FsmState<T> _currentState;
        private float _currentStateTime;
        private bool _isDestroyed;


        public string Name
        {
            get => _name;
            private set => _name = value ?? string.Empty;
        }
        public T Owner => _owner;
        public Type OwnerType => typeof(T);
        public int FsmStateCount => _states.Count;
        public bool IsRunning => _currentState != null;
        public bool IsDestroyed => _isDestroyed;
        public FsmState<T> CurrentState => _currentState;
        public float CurrentStateTime => _currentStateTime;

        public Fsm()
        {
            _owner = null;
            _states = new Dictionary<Type, FsmState<T>>();
            _currentState = null;
            _currentStateTime = 0f;
            _isDestroyed = true;
        }
        public static Fsm<T> Create(string name, T owner, List<FsmState<T>> states)
        {
            if (owner == null)
            {
                throw new Exception("FSM owner is invalid.");
            }

            if (states == null || states.Count < 1)
            {
                throw new Exception("FSM states is invalid.");
            }
            
            var fsm = new Fsm<T>();
            fsm._name = name;
            fsm._owner = owner;
            fsm._isDestroyed = false;
            foreach (var state in states)
            {
                if (state == null)
                {
                    throw new Exception("FSM states is invalid.");
                }

                var stateType = state.GetType();
                if (fsm._states.ContainsKey(stateType))
                {
                    throw new Exception($"FSM '{name}' state '{stateType.FullName}' is already exist.");
                }

                fsm._states.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        public void Clear()
        {
            if (_currentState != null)
            {
                _currentState.OnLeave(this,true);
            }

            foreach (var state in _states)
            {
                state.Value.OnDestroy(this);
            }

            _name = null;
            _owner = null;
            _states.Clear();

            _currentState = null;
            _currentStateTime = 0f;
            _isDestroyed = true;
        }
        public void Start<TState>() where TState : FsmState<T>
        {
            if (IsRunning)
            {
                throw new Exception("FSM is running, can not start again.");
            }

            var state = GetState<TState>();
            if (state == null)
            {
                throw new Exception($"FSM '{_name}' state '{typeof(TState).FullName}' is already exist.");
            }

            _currentState = state;
            _currentStateTime = 0f;
            _currentState.OnEnter(this);
        }

        public bool HasState<TState>() where TState : FsmState<T>
        {
            return _states.ContainsKey(typeof(TState));
        }

        public TState GetState<TState>() where TState : FsmState<T>
        {
            if (_states.TryGetValue(typeof(TState), out var state))
            {
                return (TState) state;
            }

            return null;
        }

        public void GetAllStates(List<FsmState<T>> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var state in _states)
            {
                results.Add(state.Value);
            }
        }

        public void Update(float logicSeconds, float realSeconds)
        {
            if (_currentState == null)
            {
                return;
            }

            _currentStateTime += logicSeconds;
            _currentState.OnUpdate(this,logicSeconds,realSeconds);
        }
        
        public void ChangeState<TState>() where TState : FsmState<T>
        {
            if (_currentState == null)
            {
                throw new Exception("Current state is invalid.");
            }

            var state = GetState<TState>();
            if (state != null)
            {
                _currentState.OnLeave(this,false);
                _currentStateTime = 0f;
                _currentState = state;
                _currentState.OnEnter(this);
            }
        }
    }
}