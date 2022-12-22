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
        [Newtonsoft.Json.JsonIgnore] public Vector3 DisPerFrame { get; set; }
    }
    
    public class TracingEnemy : IActionHandler
    {
        public void Enter(ActionNode node)
        {
            var config = (TracingEnemyConfig)node.config;
            var controller = (ActionMachineController)node.actionMachine.controller;
            var target = controller.Targeter.GetClosestTargetInAngle(config.detectDis, config.detectAngle);
            if (target)
            {
                config.DisPerFrame = (target.transform.position - controller.transform.position) * 1f / config.tracingFrames;    
            }
            else
            {
                config.DisPerFrame = Vector3.zero;
            }
        }

        public void Exit(ActionNode node)
        {
        }

        public void Update(ActionNode node, float deltaTime)
        {
            var config = (TracingEnemyConfig)node.config;
            var controller = (ActionMachineController)node.actionMachine.controller;
            var transform = controller.transform;
            transform.position = transform.position + config.DisPerFrame;
        }
    }
}