using System;
using FirstARPG.Abilities;
using FirstARPG.Inventories;
using UnityEngine;
using XMLib;
using XMLib.AM;

namespace XMLibGame
{
    [System.Serializable]
    [ActionConfig(typeof(Skill))]
    public class SkillConfig
    {
        // [SerializeReference]
        // [ActionItemTypes]
        // public Ability ActionItem;
        public int skillIndex;
        public string NextState;
    }

    public class ActionItemTypesAttribute : ObjectTypesAttribute
    {
        public override Type baseType => typeof(ActionItem);
    }
    
    public class Skill : IActionHandler
    {
        private ActionMachineController _controller;

        public void Enter(ActionNode node)
        {
            _controller = (ActionMachineController)node.actionMachine.controller;
            var config = (SkillConfig)node.config;
            _controller.ActionStore.Use(config.skillIndex,_controller.gameObject);
        }

        public void Exit(ActionNode node)
        {
        }

        public void Update(ActionNode node, float deltaTime)
        {
            if (!_controller.ActionStore.Skilling)
            {
                IActionMachine machine = node.actionMachine;
                var config = (SkillConfig)node.config;
                machine.ChangeState(config.NextState);
            }
        }
    }
}