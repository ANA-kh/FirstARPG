using UnityEngine;
using XMLib.AM;

namespace XMLibGame
{
    [System.Serializable]
    [ActionConfig(typeof(Dodge))]
    public class DodgeConfig : HoldFrames
    {
        public float speed;
        [Newtonsoft.Json.JsonIgnore]
        public Vector3 DodgeDir{get;set;}
    }
    
    public class Dodge : IActionHandler
    {
        public void Enter(ActionNode node)
        {
            var config = node.config as DodgeConfig;
            var controller = (ActionMachineController)node.actionMachine.controller;
            var movement = CalculateMovement(controller,InputData.AxisValue.normalized).normalized;
            if (movement == Vector3.zero)
            {
                movement = controller.transform.forward;
            }

            var y = Vector3.Dot(movement, controller.transform.forward);
            var xCross = Vector3.Cross(movement, controller.transform.forward);
            var x =  xCross.y > 0 ? -xCross.magnitude : xCross.magnitude;
            Debug.Log($"forward:{controller.transform.forward} movement{movement}\n xCross.y{xCross.y}FB:{y} LR:{x}");
            controller.Animator.SetFloat("FB",y);
            controller.Animator.SetFloat("LR",x);
            config.DodgeDir = movement * config.speed;
        }

        public void Exit(ActionNode node) { }

        public void Update(ActionNode node, float deltaTime)
        {
            var controller = (ActionMachineController)node.actionMachine.controller;
            var config = (DodgeConfig)node.config;
            controller.ForceReceiver.Move(config.DodgeDir, deltaTime);
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