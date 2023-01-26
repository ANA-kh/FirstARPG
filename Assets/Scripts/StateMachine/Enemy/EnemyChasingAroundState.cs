using UnityEngine;

namespace FirstARPG.StateMachine.Enemy
{
    public class EnemyChasingAroundState : EnemyBaseState
    {
        protected static readonly int SpeedHash = Animator.StringToHash("Speed");
        protected readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected readonly int DirectionHash = Animator.StringToHash("Direction");
        private bool _shouldFade;
        private float chasingAroundTime;
        private const float CrossFadeDuration = 0.2f;
        protected const float AnimatorDampTime = 0.1f;
        public EnemyChasingAroundState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.CrossFade(LocomotionHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if (!IsInChaseRange())
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }

            // if (IsInAttackRange())
            // {
            //     stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            //     return;
            // }

            FacePlayer();
            MoveAroundPlayer(deltaTime);
        }

        protected void MoveToPlayer(float deltaTime, bool forward = true)
        {
            if (stateMachine.Agent.isOnNavMesh)
            {
                if (forward)
                {
                    stateMachine.Agent.SetDestination(stateMachine.Player.transform.position);
                    Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
                    stateMachine.Animator.SetFloat(DirectionHash, 0, AnimatorDampTime, deltaTime);
                    stateMachine.Animator.SetFloat(SpeedHash, 1, AnimatorDampTime, deltaTime);
                }
                else
                {
                    stateMachine.Agent.SetDestination(stateMachine.Player.transform.position * -1);
                    Move(-stateMachine.Agent.desiredVelocity.normalized * stateMachine.ChasingAroundSpeed, deltaTime);
                    stateMachine.Animator.SetFloat(DirectionHash, 0, AnimatorDampTime, deltaTime);
                    stateMachine.Animator.SetFloat(SpeedHash, -0.4f, AnimatorDampTime, deltaTime);
                }
            }

            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        }

        private void MoveAroundPlayer(float deltaTime)
        {
            if (stateMachine.Agent.isOnNavMesh)
            {
                var dis = Vector3.Distance(stateMachine.Player.transform.position, stateMachine.transform.position);
                // if (dis <= stateMachine.ChasingAroundDis -1)
                // {
                //     MoveToPlayer(deltaTime,false);
                // }
                // else 
                if (dis <= stateMachine.ChasingAroundDis)
                {
                    chasingAroundTime += deltaTime;
                    Move(stateMachine.transform.right * stateMachine.ChasingAroundSpeed, deltaTime); //TODO 添加左右随机
                    stateMachine.Animator.SetFloat(DirectionHash, 0.9f, AnimatorDampTime, deltaTime);
                    stateMachine.Animator.SetFloat(SpeedHash, 0.4f, AnimatorDampTime, deltaTime);
                }
                else
                {
                    MoveToPlayer(deltaTime);
                }
            }
        }

        public override void Exit()
        {
            stateMachine.Agent.ResetPath();
            stateMachine.Agent.velocity = Vector3.zero;
        }
    }

    class EnemyChasingState : EnemyChasingAroundState
    {
        public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Tick(float deltaTime)
        {
            if (!IsInChaseRange())
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }

            if (IsInAttackRange())
            {
                stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
                return;
            }

            FacePlayer();
            MoveToPlayer(deltaTime);
        }
    }
}