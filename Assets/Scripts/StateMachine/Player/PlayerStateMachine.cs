using FirstARPG.InputSystem;
using UnityEngine;

namespace FirstARPG.StateMachine
{
    public class PlayerStateMachine : StateMachine
    {
        public InputReader InputReader { get; private set; }
        public CharacterController Controller { get; private set; }
        public Animator Animator { get; private set; }

        [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
        [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
        [field: SerializeField] public float RotationDamping { get; set; }

        
        public Transform MainCameraTransform { get; private set; }
        

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            InputReader = GetComponent<InputReader>();
            Controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            MainCameraTransform = Camera.main.transform;

            SwitchState(new PlayerFreeLookState(this));
        }
    }
}