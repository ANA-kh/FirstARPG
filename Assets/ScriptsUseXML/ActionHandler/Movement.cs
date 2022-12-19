using UnityEngine;
using XMLib.AM;

namespace XMLibGame
{
    [System.Serializable]
    [ActionConfig(typeof(Movement))]
    public class MoveConfig
    {
        public float moveSpeed;
    }
    public class Movement : IActionHandler
    {
        public void Enter(ActionNode node) { }

        public void Exit(ActionNode node) { }

        public void Update(ActionNode node, float deltaTime)
        {
            MoveConfig config = (MoveConfig)node.config;
            ActionMachineController controller = (ActionMachineController)node.actionMachine.controller;

            if (InputData.HasEvent(InputEvents.Moving))
            {
                var movement = CalculateMovement(controller,InputData.AxisValue.normalized);
                           //* config.moveSpeed;

                controller.ForceReceiver.Move(movement*config.moveSpeed,deltaTime);
                FaceMovementDirection(controller,movement, deltaTime);
            }
        }
        
        
        private Vector3 CalculateMovement(ActionMachineController controller,Vector2 input)
        {
            Vector3 forward = controller.MainCameraTransform.forward;
            Vector3 right = controller.MainCameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            return forward * input.y +
                   right * input.x;
        }

        private void FaceMovementDirection(ActionMachineController controller, Vector3 movement, float deltaTime)
        {
            controller.transform.rotation = Quaternion.Lerp(
                controller.transform.rotation,
                Quaternion.LookRotation(movement),
                deltaTime * controller.RotationDamping);
        }
    }
}