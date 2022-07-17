using System;
using UnityEngine;

namespace FirstARPG.StateMachine.Enemy
{
    public class EnemyAttackingState : EnemyBaseState
    {
        private readonly int AttackHash = Animator.StringToHash("ANI_Attack1");
        private const float TransitionDuration = 0.1f;
        public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Weapon.SetAttack(stateMachine.AttackDamage,stateMachine.AttackKonckback);
            stateMachine.Animator.CrossFadeInFixedTime(AttackHash,TransitionDuration);
        }

        

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(stateMachine.Animator,"Attack") >= 1f)
            {
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            }
        }

        public override void Exit()
        {
            //throw new NotImplementedException();
        }
    }
}