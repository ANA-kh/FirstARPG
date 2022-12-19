using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using XMLibGame;

namespace FirstARPG.Miscs
{
    /// <summary>
    /// 模拟受外力
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class ForceReceiver : MonoBehaviour
    {
        public bool UseLogicUpdate = true;
        private CharacterController _characterController;
        [SerializeField]
        private float drag = 0.3f;

        private Vector3 _dampingVelocity;
        private Vector3 _impact;
        private float _verticalVelocity;
        private ActionMachineController _actionMachineController;

        public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _actionMachineController = GetComponent<ActionMachineController>();
        }

        private void Update()
        {
            if (UseLogicUpdate) return;
            if (_verticalVelocity < 0f && _characterController.isGrounded)
            {
                _verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, drag);
        }

        public void LogicUpdate(float deltaTime)
        {
            if (_verticalVelocity < 0f && _characterController.isGrounded)
            {
                _verticalVelocity = Physics.gravity.y * deltaTime;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * deltaTime;
            }

            _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, drag);
        }

        public void Reset()
        {
            _impact = Vector3.zero;
            _verticalVelocity = 0f;
        }

        public void AddForce(Vector3 force)
        {
            _impact += force;
        }

        public void Jump(float jumpForce)
        {
            _verticalVelocity += jumpForce;
        }
        public void Move(Vector3 motion, float deltaTime)
        {
            _characterController.Move((motion + Movement) * deltaTime);
            //CharacterController.Move(motion * deltaTime);会在Time.deltaTime的时间内移动motion的距离
            //所以CharacterController.velocity = (motion * deltaTime)/Time.deltaTime
            //但由于逻辑帧调用move的频率小于每帧，所以实际速度小于CharacterController.velocity
            _actionMachineController.CurTrueVelocity = motion + Movement;
        }
    }
}