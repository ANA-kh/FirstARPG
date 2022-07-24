using UnityEngine;

namespace FirstARPG.StateMachine
{
    public class PlayerDodgingState : PlayerBaseState
    {
        private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
        private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
        private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");

        private float _remainingDodgeTime;
        private Vector3 _dodgingDirectionInput;

        private const float CrossFadeDuration = 0.1f;

        public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
        {
            _dodgingDirectionInput = dodgingDirectionInput;
        }

        public override void Enter()
        {
            _remainingDodgeTime = stateMachine.DodgeDuration;

            stateMachine.Animator.SetFloat(DodgeForwardHash, _dodgingDirectionInput.y);
            stateMachine.Animator.SetFloat(DodgeRightHash, _dodgingDirectionInput.x);
            stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);

            stateMachine.Health.SetGod(true);
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = new Vector3();

            movement += stateMachine.transform.right * _dodgingDirectionInput.x * stateMachine.DodgeLength / stateMachine.DodgeDuration;
            movement += stateMachine.transform.forward * _dodgingDirectionInput.y * stateMachine.DodgeLength / stateMachine.DodgeDuration;

            Move(movement, deltaTime);

            FaceTarget();

            _remainingDodgeTime -= deltaTime;

            if (_remainingDodgeTime <= 0f)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {
            stateMachine.Health.SetGod(false);
        }
    }
}