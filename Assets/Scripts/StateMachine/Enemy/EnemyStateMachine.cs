using System;
using UnityEngine;

namespace FirstARPG.StateMachine.Enemy
{
    public class EnemyStateMachine : StateMachine
    {
        public Animator Animator { get; private set; }
        public CharacterController Controller { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            SwitchState(new EnemyIdleState(this));
        }
    }
}