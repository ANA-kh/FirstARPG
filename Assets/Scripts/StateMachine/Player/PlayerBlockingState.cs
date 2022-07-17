using System;
using UnityEngine;

namespace FirstARPG.StateMachine
{
    public class PlayerBlockingState : PlayerBaseState
    {
        private readonly int BlockHash = Animator.StringToHash("ANI_Block");
        private const float CrossFadeDuration = 0.1f;
        public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Health.SetGod(true);
            stateMachine.Animator.CrossFadeInFixedTime(BlockHash,CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            if (!stateMachine.InputReader.IsBlocking)
            {
                ReturnToLocomotion();
                return;
            }

            if (!stateMachine.Animator.IsInTransition(0) && stateMachine.IkActive == false)
            {
                stateMachine.IkActive = true;
            }
        }

        public override void Exit()
        {
            stateMachine.Health.SetGod(false);
            stateMachine.IkActive = false;
        }
    }
}