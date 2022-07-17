using UnityEngine;

namespace FirstARPG.StateMachine
{
    public class PlayerImpactState : PlayerBaseState
    {
        private readonly int ImpactHash = Animator.StringToHash("ANI_Impact");
        private const float CrossFadeDuration = 0.1f;
        private float _duration = 1.0f;
        
        public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(ImpactHash,CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            _duration -= deltaTime;
            if (_duration <= 0.0f)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit() { }
    }
}