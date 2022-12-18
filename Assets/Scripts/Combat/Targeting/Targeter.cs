using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace FirstARPG.Combat
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup cineTargetGroup;
        private Camera _mainCamera;
        private List<Target> _targets = new List<Target>();

        public Target CurrentTarget { get; private set; }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target)) { return; }

            _targets.Add(target);
            target.OnDestroyed += RemoveTarget;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target)) { return; }

            RemoveTarget(target);
        }
        
        public bool SelectTarget()
        {
            if (_targets.Count == 0) { return false; }

            Target closestTarget = GetClosestTarget();

            if (closestTarget == null) { return false; }

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
            if (CurrentTarget == null) { return; }

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
    }
}