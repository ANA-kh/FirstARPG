using UnityEngine;

namespace FirstARPG.StateMachine.Enemy
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine stateMachine;
        
        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }
}