using System;
using FirstARPG.Attributes;
using FirstARPG.Combat;
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
        public Health Health { get; private set; }
        public Target Target { get;private set; }
        public Ragdoll Ragdoll { get; private set; }

        [field: SerializeField] public WeaponDamage Weapon { get; private set; }
        [field:SerializeField]public float PlayerChasingRange { get; private set; }
        [field:SerializeField]public float MovementSpeed { get; private set; }
        [field:SerializeField]public float ChasingAroundSpeed { get; private set; }
        [field:SerializeField]public float PlayerAttackingRange { get; private set; }
        [field:SerializeField]public int AttackDamage { get; private set; }
        [field:SerializeField]public float AttackKonckback { get; private set; }

        private void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            Animator = GetComponent<Animator>();
            Controller = GetComponent<CharacterController>();
            ForceReceiver = GetComponent<ForceReceiver>();
            Agent = GetComponent<NavMeshAgent>();
            Health = GetComponent<Health>();
            Target = GetComponent<Target>();
            Ragdoll = GetComponent<Ragdoll>();
        }

        private void Start()
        {
            Agent.updatePosition = false;
            Agent.updateRotation = false;
            Weapon.Owner = gameObject;
            SwitchState(new EnemyIdleState(this));
        }

        private void OnEnable()
        {
            Health.OnTakeDamage += HandleTakeDamage;
            Health.OnDie += HandleDie;
        }

        private void OnDisable()
        {
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDie;
        }

        private void HandleTakeDamage()
        {
            SwitchState(new EnemyImpactState(this));
        }
        private void HandleDie()
        {
            SwitchState(new EnemyDeadState(this));
        }
        
        public bool CheckState(Type type)
        {
            return _currentState.GetType() == type;
        }
        
        public void ChasingPlayer()
        {
            SwitchState(new EnemyChasingState(this));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,PlayerChasingRange);
        }
    }
}