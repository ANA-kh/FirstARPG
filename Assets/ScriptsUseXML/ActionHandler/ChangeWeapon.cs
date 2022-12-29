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
    }
    public class ChangeWeapon : IActionHandler
    {
        public void Enter(ActionNode node)
        {
            var controller = (ActionMachineController)node.actionMachine.controller;
            var config = (ChangeWeaponConfig)node.config;
            controller.ChangeWeapon(config.weaponIndex);
        }

        public void Exit(ActionNode node)
        {
            
        }

        public void Update(ActionNode node, float deltaTime)
        {
            
        }
    }
}