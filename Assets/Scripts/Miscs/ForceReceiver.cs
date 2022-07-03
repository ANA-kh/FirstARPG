using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FirstARPG.Miscs
{
    [RequireComponent(typeof(CharacterController))]
    public class ForceReceiver : MonoBehaviour
    {
        private CharacterController _controller;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float drag = 0.3f;

        private Vector3 _dampingVelocity;
        private Vector3 _impact;
        private float _verticalVelocity;

        public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            if (_verticalVelocity < 0f && _controller.isGrounded)
            {
                _verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, drag);

            if (agent != null)
            {
                if (_impact.sqrMagnitude < 0.2f * 0.2f)
                {
                    _impact = Vector3.zero;
                    agent.enabled = true;
                }
            }
        }

        public void Reset()
        {
            _impact = Vector3.zero;
            _verticalVelocity = 0f;
        }

        public void AddForce(Vector3 force)
        {
            _impact += force;
            if (agent != null)
            {
                agent.enabled = false;
            }
        }

        public void Jump(float jumpForce)
        {
            _verticalVelocity += jumpForce;
        }
    }
}