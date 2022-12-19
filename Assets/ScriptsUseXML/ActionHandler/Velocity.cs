using System.Collections;
using System.Collections.Generic;
using FirstARPG.Miscs;
using UnityEngine;
using XMLib.AM;

namespace XMLibGame
{
    [System.Serializable]
    [ActionConfig(typeof(Velocity))]
    public class VelocityConfig : HoldFrames
    {
        public Vector3 velocity;
    }

    public class Velocity : IActionHandler
    {
        private ForceReceiver _forceReceiver;

        public void Enter(ActionNode node)
        {
            VelocityConfig config = (VelocityConfig)node.config;
            var controller = (ActionMachineController)node.actionMachine.controller;

            _forceReceiver = controller.ForceReceiver;
            _forceReceiver.AddForce(controller.transform.rotation * config.velocity);
        }

        public void Exit(ActionNode node) { }

        public void Update(ActionNode node, float deltaTime)
        {
            _forceReceiver.Move(Vector3.zero,deltaTime);
        }
    }
}