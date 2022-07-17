using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.Combat
{
    public class Ragdoll : MonoBehaviour
    {
        private Animator _animator;
        private CharacterController _controller;

        private Collider[] _allColliders;
        private Rigidbody[] _allRigidbodies;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
            _allColliders = GetComponentsInChildren<Collider>(true);
            _allRigidbodies = GetComponentsInChildren<Rigidbody>(true);

            ToggleRagdoll(false);
        }

        public void ToggleRagdoll(bool isRagdoll)
        {
            foreach (Collider collider in _allColliders)
            {
                if (collider.gameObject.CompareTag("Ragdoll"))
                {
                    collider.enabled = isRagdoll;
                }
            }

            foreach (Rigidbody rigidbody in _allRigidbodies)
            {
                if (rigidbody.gameObject.CompareTag("Ragdoll"))
                {
                    rigidbody.isKinematic = !isRagdoll;
                    rigidbody.useGravity = isRagdoll;
                }
            }

            _controller.enabled = !isRagdoll;
            _animator.enabled = !isRagdoll;
        }
    }
}