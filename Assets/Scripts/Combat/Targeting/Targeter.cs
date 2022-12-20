using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using XMLibGame;

namespace FirstARPG.Combat
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField]
        private CinemachineTargetGroup cineTargetGroup;
        private Camera _mainCamera;
        private List<Target> _targets = new List<Target>();
        public Transform Player;

        public Target CurrentTarget { get; private set; }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target))
            {
                return;
            }

            _targets.Add(target);
            target.OnDestroyed += RemoveTarget;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target))
            {
                return;
            }

            RemoveTarget(target);
        }

        public bool SelectTarget()
        {
            if (_targets.Count == 0)
            {
                return false;
            }

            Target closestTarget = GetClosestTarget();

            if (closestTarget == null)
            {
                return false;
            }

            CurrentTarget = closestTarget;
            //OldGame
            //cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

            return true;
        }

        public Target GetClosestTarget()
        {
            Target closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach (Target target in _targets)
            {
                Vector2 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);

                if (!target.GetComponentInChildren<Renderer>().isVisible)
                {
                    continue;
                }

                Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
                if (toCenter.sqrMagnitude < closestTargetDistance)
                {
                    closestTarget = target;
                    closestTargetDistance = toCenter.sqrMagnitude;
                }
            }

            return closestTarget;
        }

        public void Cancel()
        {
            if (CurrentTarget == null)
            {
                return;
            }

            //OldGame
            //cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        private void RemoveTarget(Target target)
        {
            if (CurrentTarget == target)
            {
                //OldGame
                //cineTargetGroup.RemoveMember(CurrentTarget.transform);
                CurrentTarget = null;
            }

            target.OnDestroyed -= RemoveTarget;
            _targets.Remove(target);
        }

        public Target GetClosestTargetInAngle(float maxDis,float configDetectAngle)
        {
            Target result = null;
            var detectDirection = DetectDirection();
            if (detectDirection != Vector3.zero)
            {
                var closestAngle = configDetectAngle;

                foreach (var target in _targets)
                {
                    if (!target.GetComponentInChildren<Renderer>().isVisible)
                    {
                        continue;
                    }
                    //calculate angle between forward and target position
                    var targetDir = target.transform.position - Player.position;
                    var angle = Vector3.Angle(targetDir, detectDirection);
                    if (angle < closestAngle && targetDir.magnitude < maxDis)
                    {
                        closestAngle = angle;
                        result = target;
                    }
                }
            }

            ClosestTarget = result;
            return result;
        }

        public Target ClosestTarget { get; set; }

        private Vector3 DetectDirection()
        {
            if (_mainCamera)
            {
                var forward = _mainCamera.transform.forward;
                var right = _mainCamera.transform.right;
                forward.y = 0f;
                right.y = 0f;

                forward.Normalize();
                right.Normalize();

                var input = InputData.AxisValue.normalized;
                if (input == Vector2.zero)
                {
                    return forward;
                }
                var detectDirection = forward * input.y + right * input.x;
                return detectDirection;
            }

            return Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            var derection = DetectDirection();

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(Player.position, derection * 2);
            Gizmos.DrawWireSphere(Player.position + derection * 2, 0.2f);
            var target = GetClosestTargetInAngle(5,30);
            if (target != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(target.transform.position, 1f);
            }
        }
    }
}