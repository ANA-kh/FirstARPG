using System;
using FirstARPG.Combat;
using XMLib.AM;

namespace XMLibGame
{
    [Serializable]
    [ActionConfig(typeof(ChangeWeapon))]
    public class ChangeWeaponConfig : HoldFrames
    {
        public int weaponIndex;
        public bool changeActionMachine;
        public int actionMachineIndex;
    }
    public class ChangeWeapon : IActionHandler
    {
        public void Enter(ActionNode node)
        {
            var controller = (ActionMachineController)node.actionMachine.controller;
            var config = (ChangeWeaponConfig)node.config;
            controller.ChangeWeapon(config.weaponIndex);
            if (config.changeActionMachine)
            {
                controller.ChangeActionMachine(config.actionMachineIndex);
            }
        }

        public void Exit(ActionNode node)
        {
            
        }

        public void Update(ActionNode node, float deltaTime)
        {
            
        }
    }
}