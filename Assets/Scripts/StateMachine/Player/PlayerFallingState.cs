using UnityEngine;

namespace FirstARPG.StateMachine
{
    internal class PlayerFallingState : PlayerBaseState
    {
        private readonly int FallHash = Animator.StringToHash("ANI_NormalJumpDown");

        private Vector3 _momentum;

        private const float CrossFadeDuration = 0.1f;
        public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            _momentum = stateMachine.Controller.velocity;
            _momentum.y = 0f;

            stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(_momentum, deltaTime);

            if (stateMachine.Controller.isGrounded)
            {
                ReturnToLocomotion();
            }

            FaceTarget();
        }

        public override void Exit()
        {
        }
    }
}