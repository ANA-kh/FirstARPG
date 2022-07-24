using System;
using FirstARPG.Player;
using UnityEngine;

namespace FirstARPG.StateMachine
{
    internal class PlayerJumpingState : PlayerBaseState
    {
        private readonly int JumpHash = Animator.StringToHash("ANI_NormalJumpUp");
        
        private const float CrossFadeDuration = 0.1f;
        private Vector3 _momentum;
        private bool _parkourJump;
        public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            _parkourJump = stateMachine.ParkourController.PerformAction();
            if (!_parkourJump)
            {
                stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

                _momentum = stateMachine.CurVelocity;
                _momentum.y = 0f;

                stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
            }
        }

        public override void Tick(float deltaTime)
        {
            if (_parkourJump)
            {
                if (!stateMachine.ParkourController.InAction)
                {
                    ReturnToLocomotion();
                }
            }
            else
            {
                Move(_momentum, deltaTime);

                Debug.Log($"stateMachine.CurVelocity:{stateMachine.CurVelocity}");
                if (stateMachine.Controller.velocity.y <= 0)
                {
                    stateMachine.SwitchState(new PlayerFallingState(stateMachine));
                    return;
                }

                FaceTarget();
            }
        }

        public override void Exit()
        {
            
        }
    }
}