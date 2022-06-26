using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.Player
{
    public class ParkourController : MonoBehaviour
    {
        [SerializeField] private List<ParkourAction> parkourActions;

        private bool _inAction;

        private EnvironmentScanner _environmentScanner;
        private Animator _animator;
        private PlayerSimpleMove _playerController;

        private void Awake()
        {
            _environmentScanner = GetComponent<EnvironmentScanner>();
            _animator = GetComponent<Animator>();
            _playerController = GetComponent<PlayerSimpleMove>();
        }

        private void Update()
        {
            if (Input.GetButton("Jump") && !_inAction)
            {
                var hitData = _environmentScanner.ObstacleCheck();
                if (hitData.forwardHitFound)
                {
                    foreach (var action in parkourActions)
                    {
                        if (action.CheckIfPossible(hitData, transform))
                        {
                            StartCoroutine(DoParkourAction(action));
                            break;
                        }
                    }
                }
            }
        }

        private IEnumerator DoParkourAction(ParkourAction action)
        {
            _inAction = true;
            _playerController.SetControl(false);

            _animator.CrossFade(action.AnimName, 0.2f);
            yield return null;

            var animState = _animator.GetNextAnimatorStateInfo(0);
            if (!animState.IsName(action.AnimName))
                Debug.LogError("The parkour animation is wrong!");

            float timer = 0f;
            while (timer <= animState.length)
            {
                timer += Time.deltaTime;

                if (action.RotateToObstacle)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation,
                        _playerController.RotationSpeed * Time.deltaTime);

                if (action.EnableTargetMatching) //目标匹配
                    MatchTarget(action);

                if (_animator.IsInTransition(0) && timer > 0.5f)
                    break;

                yield return null;
            }

            yield return new WaitForSeconds(action.PostActionDelay);

            _playerController.SetControl(true);
            _inAction = false;
        }

        private void MatchTarget(ParkourAction action)
        {
            if (_animator.isMatchingTarget) return;

            _animator.MatchTarget(action.MatchPos, transform.rotation, action.MatchBodyPart,
                new MatchTargetWeightMask(action.MatchPosWeight, 0),
                action.MatchStartTime, action.MatchTargetTime);
        }
    }
}