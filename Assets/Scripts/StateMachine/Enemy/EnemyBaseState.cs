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

        protected bool IsInAttackRange()
        {
            return Vector3.Distance(stateMachine.transform.position, stateMachine.Player.transform.position) <= stateMachine.PlayerAttackingRange;
        }
        
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }
        
        protected void Move(Vector3 motion, float deltaTime)
        {
            stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
        }
        
        //FacePlayer
        protected void FacePlayer()
        {
            if (stateMachine.Player == null) { return; }
            
            
            Vector3 direction = stateMachine.Player.transform.position - stateMachine.transform.position;
            direction.y = 0;
            stateMachine.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}