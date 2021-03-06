using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FirstARPG.InputSystem
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        public bool IsAttacking { get; private set; }
        public bool IsBlocking { get; private set; }
        public Vector2 MovementValue { get; private set; }

        public event Action JumpEvent;
        public event Action DodgeEvent;
        public event Action TargetEvent;
        public event Action CancelEvent;

        private Controls _controls;

        private void Start()
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);

            _controls.Player.Enable();
        }

        private void OnDestroy()
        {
            _controls?.Player.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }

            JumpEvent?.Invoke();
            //Debug.Log("jumpTest");
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }

            DodgeEvent?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {

        }

        public void OnTarget(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }

            TargetEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }

            CancelEvent?.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsAttacking = true;
            }
            else if (context.canceled)
            {
                IsAttacking = false;
            }
        }

        public void OnBlock(InputAction.CallbackContext context)
        {
            // if (context.performed)
            // {
            //     IsBlocking = true;
            // }
            // else if (context.canceled)
            // {
            //     IsBlocking = false;
            // }
        }
    }
}