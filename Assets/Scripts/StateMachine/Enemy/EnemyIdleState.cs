using UnityEngine;

namespace FirstARPG.StateMachine.Enemy
{
    public class EnemyIdleState : EnemyBaseState
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private bool _shouldFade;
        private float _idleTime;
        private const float CrossFadeDuration = 0.2f;
        private const float AnimatorDampTime = 0.1f;

        public EnemyIdleState(EnemyStateMachine stateMachine, bool shouldFade = true,float idleTime = 0) : base(stateMachine)
        {
            _shouldFade = shouldFade;
            _idleTime = idleTime;
        }

        public override void Enter()
        {
            if (_shouldFade)
            {
                stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
            }
            else
            {
                stateMachine.Animator.Play(LocomotionHash);
            }
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            if (!stateMachine.StopEnemy&&IsInChaseRange() && _idleTime < 0)
            {
                stateMachine.SwitchState(new EnemyChasingAroundState(stateMachine));
            }
            _idleTime -= deltaTime;
            stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
        }

        public override void Exit() { }
    }
}