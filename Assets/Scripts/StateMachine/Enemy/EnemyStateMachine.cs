using System;
using FirstARPG.Miscs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace FirstARPG.StateMachine.Enemy
{
    public class EnemyStateMachine : StateMachine
    {
        public Animator Animator { get; private set; }
        public CharacterController Controller { get; private set; }
        public GameObject Player { get; private set; }
        public ForceReceiver ForceReceiver { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        
        [field:SerializeField]public float PlayerChasingRange { get; private set; }
        [field:SerializeField]public float MovementSpeed { get; set; }


        private void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            Animator = GetComponent<Animator>();
            Controller = GetComponent<CharacterController>();
            ForceReceiver = GetComponent<ForceReceiver>();
            Agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            Agent.updatePosition = false;
            Agent.updateRotation = false;
            SwitchState(new EnemyIdleState(this));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,PlayerChasingRange);
        }
    }
}