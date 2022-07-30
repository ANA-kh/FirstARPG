using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.StateMachine
{


    public class PlayerPullUpState : PlayerBaseState
    {
        private readonly int PullUpHash = Animator.StringToHash("ANI_HangingUp");
        private readonly Vector3 Offset = new Vector3(0f, 2.325f, 0.65f);

        private const float CrossFadeDuration = 0.1f;
        private Vector3 _matchPos;

        public PlayerPullUpState(PlayerStateMachine stateMachine, Vector3 matchPos) : base(stateMachine)
        {
            _matchPos = matchPos;
        }

        public override void Enter()
        {
            Debug.DrawLine(_matchPos,stateMachine.transform.forward * 3 , Color.blue, 5f);
            stateMachine.Animator.applyRootMotion = true;
            stateMachine.Controller.enabled = false;
            stateMachine.Animator.CrossFadeInFixedTime(PullUpHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(stateMachine.Animator, "Climbing") < 1f)
            {
                if (!stateMachine.Animator.IsInTransition(0))
                {
                    stateMachine.Animator.MatchTarget(_matchPos, Quaternion.identity, AvatarTarget.LeftFoot,
                        new MatchTargetWeightMask(Vector3.one, 0f), 0.5f, 0.6f);
                }
                return;
            }

            //stateMachine.transform.Translate(Offset, Space.Self);
            
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }

        public override void Exit()
        {
            stateMachine.Animator.applyRootMotion = false;
            stateMachine.Controller.enabled = true;
            stateMachine.Controller.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
        }
    }
}