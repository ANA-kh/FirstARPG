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
        
        protected bool IsInChaseRange()
        {
            return Vector3.Distance(stateMachine.transform.position, stateMachine.Player.transform.position) <= stateMachine.PlayerChasingRange;
        }
        
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }
        
        protected void Move(Vector3 motion, float deltaTime)
        {
            stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
        }
    }
}