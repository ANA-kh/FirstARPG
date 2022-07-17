using System;
using UnityEngine;

namespace FirstARPG.StateMachine.Enemy
{
    public class EnemyAttackingState : EnemyBaseState
    {
        private readonly int AttackHash = Animator.StringToHash("ANI_Attack1");
        private const float TransitionDuration = 0.1f;
        private const float BeforeAttack = 0.1f;
        private float _attackTimer;
        public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Weapon.SetAttack(stateMachine.AttackDamage,stateMachine.AttackKonckback);
            _attackTimer = BeforeAttack;
            FacePlayer();
        }

        

        public override void Tick(float deltaTime)
        {
            
            if (_attackTimer > 0)
            {
                _attackTimer -= deltaTime;
                if (_attackTimer <= 0)
                {
                    stateMachine.Animator.CrossFadeInFixedTime(AttackHash,TransitionDuration);
                }
                return;
            }
            
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