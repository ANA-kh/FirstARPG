using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XMLibGame
{
    public class InputReaderXMLib :MonoBehaviour, Controls.IPlayerActions
    {
        private Controls _controls;
        public void Init(Controls controls)
        {
            _controls = controls;
            _controls.Player.SetCallbacks(this);

            _controls.Player.Enable();
            
            _controls.Player.Move.started += (context) => {Debug.Log("started"); };
            _controls.Player.Move.performed += (context) => {Debug.Log("performed"); };
            _controls.Player.Move.canceled += (context) => {Debug.Log("canceled"); };
        }

        public void DisableCtr()
        {
            _controls?.Player.Disable();
        }
        
        public void EnableCtr()
        {
            _controls?.Player.Enable();
        }

        private void Update()
        {
            //Debug.Log(_controls.Player.Move.phase.ToString());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.action.triggered)
            {
                InputData.InputEvents |= InputEvents.Jump;
            }
            else if (context.phase == InputActionPhase.Started)
            {
                InputData.InputEvents |= InputEvents.Jumping;
            }
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            //Debug.Log(context.action.phase.ToString());
            if (context.action.phase == InputActionPhase.Started)
            {
                InputData.InputEvents |= InputEvents.Moving;
                InputData.AxisValue = context.ReadValue<Vector2>();
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            
        }

        public void OnTarget(InputAction.CallbackContext context)
        {
            
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.action.triggered)
            {
                InputData.InputEvents |= InputEvents.Attack;
            }
        }

        public void OnBlock(InputAction.CallbackContext context)
        {
            
        }

        public void OnSkill1(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnSkill2(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }
    }
}