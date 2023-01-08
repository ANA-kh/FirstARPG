using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using XMLib;
using XMLib.AM;

namespace XMLibGame
{
    [Flags]
    public enum InputEvents
    {
        None = 0b0000,
        Moving = 0b0001,
        Attack = 0b0010,
        Jump = 0b0100,
        Jumping = 0b1000,
        Skill1 = 0b1_0000,
        Skill2 = 0b10_0000,
        Tab = 0b100_0000,
        Dodge = 0b1000_0000,
    }

    public static class InputData
    {
        public static InputEvents InputEvents { get; set; } = InputEvents.None;
        public static Vector2 AxisValue { get; set; } = Vector2.zero;

        public static bool HasEvent(InputEvents e, bool fullMatch = false)
        {
            // if (InputEvents == InputEvents.None)
            // {
            //     return false;
            // }
            return fullMatch ? ((InputEvents & e) == e) : ((InputEvents & e) != 0);
        }

        public static void Clear()
        {
            InputEvents = InputEvents.None;
            AxisValue = Vector2.zero;
        }
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        protected List<ActionMachineController> controllers;

        [SerializeField]
        protected CinemachineBrain _brain;

        [SerializeField]
        protected List<TextAsset> configs;

        protected float logicTimer = 0f;
        protected const float logicDeltaTime = 1 / 60f;

        #region Input

        public Controls Input;

        #endregion Input

        private void Awake()
        {
            //初始化配置文件加载函数
            ActionMachineHelper.Init(OnActionMachineConfigLoader);
            Input = new Controls();
            Input.Enable();

            Physics.autoSimulation = false;
        }

        private MachineConfig OnActionMachineConfigLoader(string configName)
        {
            TextAsset asset = configs.Find(t => string.Compare(t.name, configName) == 0);
            return DataUtility.FromJson<MachineConfig>(asset.text);
        }

        private void OnDestroy()
        {
            Input.Disable();
        }

        private void Update()
        {
            UpdateInput();
            LogicUpdate();
        }

        private void UpdateInput()
        {
            var player = Input.Player;
            var move = player.Move.ReadValue<Vector2>();
            if (player.Move.phase == InputActionPhase.Started)
            {
                InputData.InputEvents |= InputEvents.Moving;
                InputData.AxisValue = move;
            }

            if (player.Attack.triggered)
            {
                InputData.InputEvents |= InputEvents.Attack;
            }

            if (player.Jump.triggered)
            {
                InputData.InputEvents |= InputEvents.Jump;
            }

            if (player.Jump.phase == InputActionPhase.Started)
            {
                InputData.InputEvents |= InputEvents.Jumping;
            }

            if (player.Skill1.triggered)
            {
                InputData.InputEvents |= InputEvents.Skill1;
            }

            if (player.Skill2.triggered)
            {
                InputData.InputEvents |= InputEvents.Skill2;
            }

            if (player.Target.triggered)
            {
                InputData.InputEvents |= InputEvents.Tab;
            }

            if (player.Dodge.triggered)
            {
                InputData.InputEvents |= InputEvents.Dodge;
            }
        }

        private void LogicUpdate()
        {
            logicTimer += Time.deltaTime;
            if (logicTimer >= logicDeltaTime)
            {
                logicTimer -= logicDeltaTime;

                RunLogicUpdate(logicDeltaTime);
                _brain.ManualUpdate(); //手动更新相机，解决相机抖动的问题
            }
        }

        private void RunLogicUpdate(float logicDeltaTime)
        {
            foreach (var item in controllers)
            {
                item.LogicUpdate(logicDeltaTime);
            }

            //更新物理
            Physics.Simulate(logicDeltaTime);

            //清理输入
            InputData.Clear();
        }
    }
}