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
            //_momentum = stateMachine.Controller.velocity;
            //由于Character Controller 的velocity要在调用move后才能获取对的值;
            //所以在调用move后立刻缓存了下速度
            _momentum = stateMachine.CurVelocity;
            _momentum.y = 0f;

            stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
            
            stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
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
            stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
        }
        
        private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
        {
            stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
        }
    }
}