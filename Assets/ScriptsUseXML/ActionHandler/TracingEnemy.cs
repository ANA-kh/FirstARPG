using System;
using System.Collections.Generic;
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
        public bool justShowWeapon;
        public int delayFrame;
        [Newtonsoft.Json.JsonIgnore] public Vector3 DisPerFrame { get; set; }
        [Newtonsoft.Json.JsonIgnore] public Vector3 DisPerFrameExcludeDelay { get; set; }
        [Newtonsoft.Json.JsonIgnore] public GameObject WeaponParent { get; set; }
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
                var configDisPerFrame = (target.transform.position - controller.transform.position) * 1f /
                                        config.tracingFrames;
                configDisPerFrame.y = 0;
                config.DisPerFrame = configDisPerFrame;
            }
            else
            {
                config.DisPerFrame = Vector3.zero;
            }
            config.DisPerFrameExcludeDelay = config.DisPerFrame * config.tracingFrames / (config.tracingFrames - config.delayFrame);

            if (config.justShowWeapon)
            {
                var weapon = controller.WeaponHandler.GetCurrentWeapon();
                config.WeaponParent = weapon.transform.parent.gameObject;
                weapon.transform.SetParent(null, true);
                controller.ShowBody(false);
                weapon.transform.rotation = Quaternion.LookRotation(config.DisPerFrame);
                controller.blueTrail.transform.position = weapon.transform.position;
                controller.whiteTrail.transform.rotation = weapon.transform.rotation;
                controller.blueTrail.Play();
                controller.whiteTrail.Play();
            }
        }

        public void Exit(ActionNode node)
        {
            var config = (TracingEnemyConfig)node.config;
            var controller = (ActionMachineController)node.actionMachine.controller;
            var transform = controller.transform;
            transform.position = transform.position + config.DisPerFrame * config.delayFrame;
            if (config.justShowWeapon)
            {
                controller.ShowBody(true);
                var weapon = controller.WeaponHandler.GetCurrentWeapon();
                weapon.transform.parent = config.WeaponParent.transform;
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localEulerAngles = new Vector3(0, -90, 90);
                weapon.transform.localScale = Vector3.one;
                controller.blueTrail.Stop();
                controller.whiteTrail.Stop();

                //相机震动
                controller.Impulse.GenerateImpulse(Vector3.right);
            }
        }

        public void Update(ActionNode node, float deltaTime)
        {
            var config = (TracingEnemyConfig)node.config;

            var controller = (ActionMachineController)node.actionMachine.controller;
            if (node.actionMachine.frameIndex - node.beginFrameIndex - config.GetBeginFrame() >= config.delayFrame)
            {
                var transform = controller.transform;
                transform.position = transform.position + config.DisPerFrame;
            }

            if (config.justShowWeapon)
            {
                var weapon = controller.WeaponHandler.GetCurrentWeapon();
                weapon.transform.position = weapon.transform.position + config.DisPerFrame;
                //var target = controller.Targeter.GetClosestTargetInAngle(config.detectDis, config.detectAngle);
            }
        }
    }
}