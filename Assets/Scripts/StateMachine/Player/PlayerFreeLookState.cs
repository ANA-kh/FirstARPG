using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace FirstARPG.StateMachine
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        private bool _shouldFade;

        private readonly int FreeLookBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int FreeLookSpeedHash = Animator.StringToHash("moveAmount");

        private const float AnimatorDampTime = 0.1f;

        private const float CrossFadeDuration = 0.25f;

        public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
        {
            this._shouldFade = shouldFade;
        }

        public override void Enter()
        {
            stateMachine.InputReader.TargetEvent += OnTarget;
            stateMachine.InputReader.JumpEvent += OnJump;

            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f);

            if (_shouldFade)
            {
                
                Debug.Log($"time{stateMachine.Animator.GetCurrentAnimatorStateInfo(0).length-CrossFadeDuration}");
                stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
            }
            else
            {
                stateMachine.Animator.Play(FreeLookBlendTreeHash);
            }
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.InputReader.IsAttacking)
            {
                stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
                return;
            }

            Vector3 movement = CalculateMovement();

            Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

            if (stateMachine.InputReader.MovementValue == Vector2.zero)
            {
                stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
                return;
            }

            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

            FaceMovementDirection(movement, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.InputReader.TargetEvent -= OnTarget;
            stateMachine.InputReader.JumpEvent -= OnJump;
        }

        private void OnTarget()
        {
            if (!stateMachine.Targeter.SelectTarget())
            {
                return;
            }

            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }

        private void OnJump()
        {
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
        }

        private Vector3 CalculateMovement()
        {
            Vector3 forward = stateMachine.MainCameraTransform.forward;
            Vector3 right = stateMachine.MainCameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            return forward * stateMachine.InputReader.MovementValue.y +
                   right * stateMachine.InputReader.MovementValue.x;
        }

        private void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            stateMachine.transform.rotation = Quaternion.Lerp(
                stateMachine.transform.rotation,
                Quaternion.LookRotation(movement),
                deltaTime * stateMachine.RotationDamping);
        }
    }
}