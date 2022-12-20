using System;
using FirstARPG.Combat;
using UnityEngine;
using XMLib.AM;

namespace XMLibGame
{
    [System.Serializable]
    [ActionConfig(typeof(TracingEnemy))]
    public class TracingEnemyConfig : HoldFrames
    {
        public int tracingFrames = 1;
        public float detectDis;
        public float detectAngle;
        [Newtonsoft.Json.JsonIgnore]
        public Target Target{get; set; }
        [Newtonsoft.Json.JsonIgnore] public float frameCount { get; set; } = 0;
    }
    
    public class TracingEnemy : IActionHandler
    {
        public void Enter(ActionNode node)
        {
            var config = (TracingEnemyConfig)node.config;
            var controller = (ActionMachineController)node.actionMachine.controller;
            controller.Targeter.GetClosestTargetInAngle(config.detectDis, config.detectAngle);
            config.Target = controller.Targeter.ClosestTarget;
            config.frameCount = 0;
        }

        public void Exit(ActionNode node)
        {
        }

        public void Update(ActionNode node, float deltaTime)
        {
            var config = (TracingEnemyConfig)node.config;
            config.frameCount++;
            if (config.Target)
            {
                var controller = (ActionMachineController)node.actionMachine.controller;
                controller.transform.position = Vector3.Lerp(controller.transform.position, config.Target.transform.position, 2.0f/config.tracingFrames);
            }
        }
    }
}