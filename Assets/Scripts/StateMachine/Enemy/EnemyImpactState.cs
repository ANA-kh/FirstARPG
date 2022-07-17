using UnityEngine;

namespace FirstARPG.StateMachine.Enemy
{
    public class EnemyImpactState : EnemyBaseState
    {
        private readonly int ImpactHash = Animator.StringToHash("ANI_Impact");
        private const float CrossFadeDuration = 0.1f;
        private float _duration = 1f;
        
        public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(ImpactHash,CrossFadeDuration);
            stateMachine.Agent.enabled = false;
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            _duration -= deltaTime;
            if (_duration <= 0f)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            }
        }

        public override void Exit()
        {
            stateMachine.Agent.enabled = true;
        }
    }
}