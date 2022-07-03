using UnityEngine;

namespace FirstARPG.StateMachine
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        private readonly int FreeLookSpeedHash = Animator.StringToHash("moveAmount");

        private const float AnimatorDampTime = 0.1f;

        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {

        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = CalculateMovement();

            _stateMachine.Controller.Move(movement * _stateMachine.FreeLookMovementSpeed * deltaTime);

            if (_stateMachine.InputReader.MovementValue == Vector2.zero)
            {
                _stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
                return;
            }

            _stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

            FaceMovementDirection(movement, deltaTime);
        }

        public override void Exit()
        {

        }

        private Vector3 CalculateMovement()
        {
            Vector3 forward = _stateMachine.MainCameraTransform.forward;
            Vector3 right = _stateMachine.MainCameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            return forward * _stateMachine.InputReader.MovementValue.y +
                   right * _stateMachine.InputReader.MovementValue.x;
        }

        private void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            _stateMachine.transform.rotation = Quaternion.Lerp(
                _stateMachine.transform.rotation,
                Quaternion.LookRotation(movement),
                deltaTime * _stateMachine.RotationDamping);
        }
    }

}