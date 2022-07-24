using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.Player
{
    public class ParkourController : MonoBehaviour
    {
        [SerializeField] private List<ParkourAction> parkourActions;
        private const float CrossFadeDuration = 0.25f;//与状态机中动画的crossTime一致  TODO  易混乱，后续修改

        public bool InAction { get; private set; }

        public float RotationDamping
        {
            get => _rotationDamping;
            set => _rotationDamping = value;
        }

        private EnvironmentScanner _environmentScanner;
        private Animator _animator;
        private CharacterController _characterController;
        private float _rotationDamping;
        private void Awake()
        {
            _environmentScanner = GetComponent<EnvironmentScanner>();
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
        }

        public bool PerformAction()
        {
            bool result = false;
            var hitData = _environmentScanner.ObstacleCheck();
            if (hitData.forwardHitFound)
            {
                foreach (var action in parkourActions)
                {
                    if (action.CheckIfPossible(hitData, transform))
                    {
                        StartCoroutine(DoParkourAction(action));
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }


        private IEnumerator DoParkourAction(ParkourAction action)
        {
            InAction = true;
            _animator.applyRootMotion = true;
            _characterController.enabled = false;

            _animator.CrossFade(action.AnimName, 0.2f);
            yield return null;

            var animState = _animator.GetNextAnimatorStateInfo(0);
            if (!animState.IsName(action.AnimName))
                Debug.LogError("The parkour animation is wrong!");

            float timer = 0f;
            while (timer <= animState.length - CrossFadeDuration)
            {
                timer += Time.deltaTime;

                if (action.RotateToObstacle)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation,
                        _rotationDamping * Time.deltaTime);

                if (action.EnableTargetMatching) //目标匹配
                    MatchTarget(action);

                if (_animator.IsInTransition(0) && timer > 0.5f)
                    break;

                yield return null;
            }

            yield return new WaitForSeconds(action.PostActionDelay);
            
            InAction = false;
            _animator.applyRootMotion = false;
            _characterController.enabled = true;
        }

        private void MatchTarget(ParkourAction action)
        {
            if (_animator.isMatchingTarget) return;

            Debug.DrawLine(action.MatchPos,new Vector3(action.MatchPos.x - 1,action.MatchPos.y,action.MatchPos.z),Color.red,5);
            _animator.MatchTarget(action.MatchPos, action.TargetRotation, action.MatchBodyPart,
                new MatchTargetWeightMask(action.MatchPosWeight, 0),
                action.MatchStartTime, action.MatchTargetTime);
        }
    }
}