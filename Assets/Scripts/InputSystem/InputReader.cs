using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FirstARPG.InputSystem
{
    public class InputReader :MonoBehaviour ,Controls.IPlayerActions
    {
        private Controls _controls;
        public event Action JumpEvent;
        public event Action DodgeEvent;

        private void Start()
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
            _controls.Player.Enable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }

            JumpEvent?.Invoke();
            Debug.Log("jumpTest");
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }

            DodgeEvent?.Invoke();
        }
    }
}