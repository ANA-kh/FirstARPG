namespace FirstARPG.StateMachine
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private Attack _attack;
        private float _previousFrameTime;
        private bool _alreadyAppliedForce = false;

        public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
        {
            _attack = stateMachine.Attacks[attackIndex];
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionDuration);
            stateMachine.Weapon.SetAttack(_attack.Damage,_attack.Force);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            FaceTarget();
            
            var normalizedTime = GetNormalizedTime();

            if (normalizedTime >= _previousFrameTime && normalizedTime < 1f)
            {
                if (normalizedTime >= _attack.ForceTime)
                {
                    TryApplyForce();
                }
                
                if (stateMachine.InputReader.IsAttacking)
                {
                    TryComboAttack(normalizedTime);
                }
            }
            else
            {
                if (stateMachine.Targeter.CurrentTarget != null)
                {
                    stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                }
                else
                {
                    stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                }
            }

            _previousFrameTime = normalizedTime;
        }

        private void TryApplyForce()
        {
            if (_alreadyAppliedForce)
            {
                return;
            }
            stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * _attack.Force);
            _alreadyAppliedForce = true;
        }

        private void TryComboAttack(float normalizedTime)
        {
            if (_attack.ComboStateIndex == -1)
            {
                return;
            }

            if (normalizedTime < _attack.ComboAttackTime)
            {
                return;
            }
            
            stateMachine.SwitchState(
                new PlayerAttackingState(
                    stateMachine,_attack.ComboStateIndex));
        }

        public override void Exit()
        {
            
        }

        private float GetNormalizedTime()
        {
            var currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            var nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

            if (stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
            {
                return nextInfo.normalizedTime;
            }
            else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
            {
                return currentInfo.normalizedTime;
            }
            else
            {
                return 0;
            }
        }
    }
}