using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using XMLib.AM;

namespace XMLibGame
{


    [System.Serializable]
    [ActionConfig(typeof(Jump))]
    public class JumpConfig
    {
        public float JumpForce;
        public string nextState;
    }

    public class Jump : IActionHandler
    {
        private Vector3 _momentum;
        private ActionMachineController _controller;
        private JumpConfig _config;
        private IActionMachine _machine;

        public void Enter(ActionNode node)
        {
            _config = (JumpConfig)node.config;
            _machine = node.actionMachine;
            _controller = (ActionMachineController)node.actionMachine.controller;
            _momentum = _controller.CurTrueVelocity;
            _momentum.y = 0;
            _controller.ForceReceiver.Jump(_config.JumpForce);
        }

        public void Exit(ActionNode node) { }

        public void Update(ActionNode node, float deltaTime)
        {
            _controller.ForceReceiver.Move(_momentum, deltaTime);
            if (_controller.isGround)
            {//落地跳转
                _machine.ChangeState(_config.nextState);
            }
        }
    }
}