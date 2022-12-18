using FirstARPG.Combat;
using UnityEngine;
using XMLib.AM;

namespace XMLibGame
{
    [System.Serializable]
    [ActionConfig(typeof(FacingTarget))]
    public class FacingTargetConfig : HoldFrames
    {
        public float RotationDamping;
    }
    
    public class FacingTarget : IActionHandler
    {
        private Target _target;
        private ActionMachineController _controller;
        private FacingTargetConfig _config;

        public void Enter(ActionNode node)
        {
            
            _controller = (ActionMachineController)node.actionMachine.controller;
            _target = _controller.Targeter.GetClosestTarget();
            _config = node.config as FacingTargetConfig;
        }

        public void Exit(ActionNode node)
        {
            
        }

        public void Update(ActionNode node, float deltaTime)
        {
            if (_target)
            {
                var lookPos = _target.transform.position - _controller.transform.position;
                lookPos.y = 0;
                _controller.transform.rotation =Quaternion.Lerp(_controller.transform.rotation,Quaternion.LookRotation(lookPos),deltaTime *_config.RotationDamping);
            }
        }
    }
}