using System;

namespace FSM
{
    public abstract class FsmState<T> where T : class
    {
        public FsmState()
        {
            
        }

        public virtual void OnInit(IFsm<T> fsm)
        {
        }
        
        public virtual void OnEnter(IFsm<T> fsm)
        {
        }
        
        public virtual void OnUpdate(IFsm<T> fsm, float logicSeconds, float realSeconds)
        {
        }
        
        public virtual void OnLeave(IFsm<T> fsm, bool isShutdown)
        {
        }
        
        public virtual void OnDestroy(IFsm<T> fsm)
        {
        }
        
        protected void ChangeState<TState>(IFsm<T> fsm) where TState : FsmState<T>
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null)
            {
                throw new Exception("FSM is invalid.");
            }

            fsmImplement.ChangeState<TState>();
        }
    }
}