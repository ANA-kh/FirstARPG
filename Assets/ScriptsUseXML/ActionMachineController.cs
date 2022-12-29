using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FirstARPG.Combat;
using FirstARPG.Inventories;
using FirstARPG.Miscs;
using UnityEngine;
using XMLib;
using XMLib.AM;
using XMLib.AM.Ranges;

namespace XMLibGame
{
    public class ActionMachineController : MonoBehaviour
    {
        [SerializeField]
        private List<string> configNames = null;

        [field: SerializeField] public Animator Animator { get;private set; }

        [SerializeField]
        private Rigidbody _rigid = null;

        [SerializeField]
        private LayerMask goundMask;

        //---
        public Transform MainCameraTransform { get; private set; }
        public CharacterController CharacterController;
        public Vector3 CurTrueVelocity { get; set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public WeaponDamage Weapon { get; private set; }
        [field: SerializeField] public float RotationDamping { get; private set; }
        [field: SerializeField] public WeaponHandler WeaponHandler { get; private set; }
        [field: SerializeField] public Targeter Targeter { get; private set; }
        [field: SerializeField] public ActionStore ActionStore { get; private set; }
        [field: SerializeField] public GameObject Model { get; private set; }
        [field: SerializeField] public Material GlowMaterial { get; private set; }
        [field: SerializeField] public CinemachineImpulseSource Impulse { get; set; }
        [Header("Particles")]
        public ParticleSystem blueTrail;
        public ParticleSystem whiteTrail;
        [Header("Slice")]
        public Transform CutPlane;
        public ParticleSystem[] SliceParticles;
        //

        private IActionMachine actionMachine;
        private float animatorTimer;

        public Rigidbody rigid => _rigid;

        public bool isGround =>
            CharacterController.isGrounded; //_isGround && Mathf.Approximately(_rigid.velocity.y, 0);
        

        private void Start()
        {
            animatorTimer = 0;

            actionMachine = new ActionMachine();
            actionMachine.Initialize(configNames[0], this);
            MainCameraTransform = Camera.main.transform;
            Animator.enabled = false;
            Weapon.Owner = gameObject;

            InitAnimation();
        }

        public void ChangeWeapon(int index)
        {
            Weapon = WeaponHandler.ChangeWeapon(index);
            Weapon.Owner = gameObject;
        }

        public void ChangeActionMachine(int index)
        {
            actionMachine.Initialize(configNames[index], this);
            InitAnimation();
        }

        private void Update()
        {
            if (!UseLogicAnimationUpdate)
            {
                UpdateAnimation();
            }
            //UpdateRotation(); 旋转放到FaceMovementDirection
        }

        public bool UseLogicAnimationUpdate;

        public void LogicUpdate(float deltaTime)
        {
            if (UseLogicAnimationUpdate)
            {
                LogicUpdateAnimation(deltaTime);
            }
            
            //更新状态
            actionMachine.LogicUpdate(deltaTime);

            //更新动画
            UpdateLogicAnimation(deltaTime);

            //更新模拟受力（由于characterController不受一些物理作用影响）
            ForceReceiver.LogicUpdate(deltaTime);
            //TODO CheckGround();
        }
        
        public void ShowBody(bool state)
        {
            var skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var smr in skinMeshList)
            {
                smr.enabled = state;
            }
            
            var meshList = GetComponentsInChildren<MeshRenderer>();
            foreach (var mr in meshList)
            {
                mr.enabled = state;
            }
        }

        private void InitAnimation()
        {
            if (Animator == null)
            {
                return;
            }

            string animName = actionMachine.GetAnimName();
            Animator.Play(animName, 0, 0);
            Animator.Update(0);
        }

        private void UpdateAnimation()
        {
            if (animatorTimer <= 0)
            {
                return;
            }

            float deltaTime = Time.deltaTime;
            if (deltaTime < animatorTimer)
            {
                animatorTimer -= deltaTime;
            }
            else
            {
                deltaTime = animatorTimer;
                animatorTimer = 0f;
            }

            if (Animator != null)
            {
                Animator.Update(deltaTime);
            }
        }
        
        private void LogicUpdateAnimation(float deltaTime)
        {
            if (animatorTimer <= 0)
            {
                return;
            }
            
            if (deltaTime < animatorTimer)
            {
                animatorTimer -= deltaTime;
            }
            else
            {
                deltaTime = animatorTimer;
                animatorTimer = 0f;
            }

            if (Animator != null)
            {
                Animator.Update(deltaTime);
            }
        }

        private void UpdateLogicAnimation(float deltaTime)
        {
            ActionMachineEvent eventTypes = actionMachine.eventTypes;

            if ((eventTypes & ActionMachineEvent.FrameChanged) != 0)
            {
                animatorTimer += deltaTime;
            }

            if ((eventTypes & ActionMachineEvent.StateChanged) != 0)
            {
                Debug.Log($"StateChanged：{actionMachine.stateName}");
            }

            if (Animator != null && (eventTypes & ActionMachineEvent.AnimChanged) != 0)
            {
                StateConfig config = actionMachine.GetStateConfig();

                float fixedTimeOffset = actionMachine.animStartTime;
                float fadeTime = config.fadeTime;
                string animName = actionMachine.GetAnimName();

                if ((eventTypes & ActionMachineEvent.HoldAnimDuration) != 0)
                {
                    fixedTimeOffset = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                }

                if (animName != string.Empty)
                {
                    Animator.CrossFadeInFixedTime(animName, fadeTime, 0, fixedTimeOffset);
                }
                Animator.Update(0);
            }
        }

        private void OnDrawGizmos()
        {
            if (actionMachine == null)
            {
                return;
            }

            Matrix4x4 mat = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            var attackRanges = actionMachine.GetAttackRanges();
            var bodyRanges = actionMachine.GetBodyRanges();
            //DrawRanges(attackRanges, mat, Color.red);
            DrawRanges(bodyRanges, mat, Color.green);

            return;

            void DrawRanges(List<RangeConfig> ranges, Matrix4x4 matrix, Color color)
            {
                if (ranges == null || ranges.Count == 0)
                {
                    return;
                }

                DrawUtility.G.PushColor(color);

                foreach (var range in ranges)
                {
                    switch (range.value)
                    {
                        case BoxItem v:
                            DrawUtility.G.DrawBox(v.size,
                                matrix * Matrix4x4.TRS((Vector3)v.offset, Quaternion.identity, Vector3.one));
                            if (color == Color.red)
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.matrix = matrix;
                                Gizmos.DrawCube(v.offset, v.size);
                            }

                            break;

                        case SphereItem v:
                            DrawUtility.G.DrawSphere(v.radius,
                                matrix * Matrix4x4.TRS((Vector3)v.offset, Quaternion.identity, Vector3.one));
                            break;
                    }
                }

                DrawUtility.G.PopColor();
            }
        }

        public void TakeDamage()
        {
            Debug.Log($"{gameObject.name} Take Damage ");
        }
    }
}