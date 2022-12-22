using System.Collections.Generic;
using XMLib.AM;
using XMLib.AM.Ranges;

namespace XMLibGame
{
    [System.Serializable]
    [ActionConfig(typeof(Attack))]
    public class AttackConfig : HoldFrames
    {
        public int Damage;
        public float Knockback = 5;
    }

    public class Attack : IActionHandler
    {

        public void Enter(ActionNode node)
        {
            var config = (AttackConfig)node.config;
            var controller = (ActionMachineController)node.actionMachine.controller;
            controller.WeaponHandler.EnableWeapon();
            controller.Weapon.SetAttack(config.Damage,config.Knockback);
        }

        public void Exit(ActionNode node)
        {
            var controller = (ActionMachineController)node.actionMachine.controller;
            controller.WeaponHandler.DisableWeapon();
        }

        public void Update(ActionNode node, float deltaTime)
        {
            var config = (AttackConfig)node.config;
            var machine = node.actionMachine;
            var controller = (ActionMachineController)node.actionMachine.controller;
            
            
            
            //伤害判定
            // var ranges = machine.GetAttackRanges();
            // Matrix4x4 mat = Matrix4x4.TRS(controller.transform.position, controller.modelRotation, Vector3.one);
            // foreach (var rangeConfig in ranges)
            // {
            //     RaycastHit[] hits = null;
            //     switch (rangeConfig.value)
            //     {
            //         case BoxItem v:
            //             hits = Physics.BoxCastAll(mat.MultiplyPoint(v.offset), (v.size / 2),
            //                 controller.transform.forward, controller.modelRotation, 0);
            //         {
            //             ExtDebug.DrawBoxCastBox(mat.MultiplyPoint(v.offset), (v.size / 2), controller.modelRotation,
            //                 controller.transform.forward, 0, Color.blue);
            //         }
            //
            //             break;
            //         case SphereItem v:
            //             hits = Physics.SphereCastAll(controller.transform.position + v.offset, v.radius, Vector3.zero);
            //             break;
            //     }
            //
            //
            //     if (hits != null)
            //     {
            //         foreach (var hit in hits)
            //         {
            //             if (targets.Contains(hit.collider))
            //             {
            //                 continue;
            //             }
            //
            //             if (hit.collider.TryGetComponent<ActionMachineController>(out var target))
            //             {
            //                 //targets.Add(hit.collider);
            //                 if (target != controller)
            //                 {
            //                     targets.Add(hit.collider);
            //                     target.TakeDamage();
            //                 }
            //             }
            //         }
            //     }
            // }
        }
    }
}