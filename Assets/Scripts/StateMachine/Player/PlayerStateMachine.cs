using FirstARPG.Attributes;
using FirstARPG.Combat;
using FirstARPG.InputSystem;
using FirstARPG.Miscs;
using FirstARPG.Player;
using FirstARPG.StateMachine.Enemy;
using UnityEngine;

namespace FirstARPG.StateMachine
{
    public class PlayerStateMachine : StateMachine
    {
        public InputReader InputReader { get; private set; }
        public CharacterController Controller;//{ get; private set; }
        public Animator Animator { get; private set; }
        public Targeter Targeter { get; private set; }
        public ForceReceiver ForceReceiver { get; private set; }
        public Health Health { get; private set; }
        public Ragdoll Ragdoll { get; private set; }
        public ParkourController ParkourController{ get; private set; }
        public LedgeDetector LedgeDetector { get; private set; }

        [field: SerializeField]public WeaponDamage Weapon { get; private set; }
        [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
        [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
        [field: SerializeField] public float RotationDamping { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public GameObject LeftHandIK { get; private set; }
        [field: SerializeField] public Attack[] Attacks { get; private set; }
        [field: SerializeField] public float DodgeDuration { get; private set; }
        [field: SerializeField] public float DodgeLength { get; private set; }
        
        
        public Vector3 CurVelocity { get; set; }


        public Transform MainCameraTransform { get; private set; }
        

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            InputReader = GetComponent<InputReader>();
            Controller = GetComponent<CharacterController>();
            Targeter = GetComponentInChildren<Targeter>();
            ForceReceiver = GetComponent<ForceReceiver>();
            Health = GetComponent<Health>();
            Ragdoll = GetComponent<Ragdoll>();
            ParkourController = GetComponent<ParkourController>();
            LedgeDetector = transform.Find("LedgeDetect").GetComponent<LedgeDetector>();
        }

        private void Start()
        {
            MainCameraTransform = Camera.main.transform;
            Weapon.Owner = gameObject;
            ParkourController.RotationDamping = RotationDamping;

            SwitchState(new PlayerFreeLookState(this));
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
            SwitchState(new PlayerImpactState(this));
        }
        private void HandleDie()
        {
            SwitchState(new PlayerDeadState(this));
        }
        
        //TODO  考虑IK独立为单独模块
        public bool IkActive { get; set; }
        private void OnAnimatorIK(int layerIndex)
        {
            if (LeftHandIK != null)
            {
                if (IkActive)
                {
                    Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
                    Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,1);
                    Animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIK.transform.position);
                    Animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandIK.transform.rotation);
                }
                else
                {
                    Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,0);
                    Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,0);
                }
            } 
        }

        protected override void OnUpdate()
        {
            //Debug.Log($"velocity{Controller.velocity}");
        }
    }
}