using System;
using UnityEngine;

namespace FirstARPG.StateMachine.Enemy
{
    public class EnemyChasingState : EnemyBaseState
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private bool _shouldFade;
        private const float CrossFadeDuration = 0.2f;
        private const float AnimatorDampTime = 0.1f;
        public EnemyChasingState(EnemyStateMachine stateMachine):base(stateMachine){ }

        public override void Enter()
        {
            stateMachine.Animator.CrossFade(LocomotionHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if (!IsInChaseRange())
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }

            MoveToPlayer(deltaTime);
            stateMachine.Animator.SetFloat(SpeedHash, 1f,AnimatorDampTime, deltaTime);
        }

        private void MoveToPlayer(float deltaTime)
        {
            stateMachine.Agent.SetDestination(stateMachine.Player.transform.position);
            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        }

        public override void Exit()
        {
            stateMachine.Agent.ResetPath();
            stateMachine.Agent.velocity = Vector3.zero;
        }
    }
}