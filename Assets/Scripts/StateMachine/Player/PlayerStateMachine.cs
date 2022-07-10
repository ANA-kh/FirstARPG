using FirstARPG.Combat;
using FirstARPG.InputSystem;
using FirstARPG.Miscs;
using UnityEngine;

namespace FirstARPG.StateMachine
{
    public class PlayerStateMachine : StateMachine
    {
        public InputReader InputReader { get; private set; }
        public CharacterController Controller { get; private set; }
        public Animator Animator { get; private set; }
        public Targeter Targeter { get; private set; }
        public ForceReceiver ForceReceiver { get; private set; }

        [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
        [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
        [field: SerializeField] public float RotationDamping { get; private set; }
        [field: SerializeField] public Attack[] Attacks { get; private set; }
        
        
        public Transform MainCameraTransform { get; private set; }
        

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            InputReader = GetComponent<InputReader>();
            Controller = GetComponent<CharacterController>();
            Targeter = GetComponentInChildren<Targeter>();
            ForceReceiver = GetComponent<ForceReceiver>();
        }

        private void Start()
        {
            MainCameraTransform = Camera.main.transform;

            SwitchState(new PlayerFreeLookState(this));
        }
    }
}