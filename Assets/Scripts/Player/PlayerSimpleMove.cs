using FirstARPG.Cameras;
using UnityEngine;

namespace FirstARPG.Player
{
    public class PlayerSimpleMove : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 500f;

        [Header("Ground Check Settings")] 
        [SerializeField] private float groundCheckRadius = 0.2f;

        [SerializeField] private Vector3 groundCheckOffset;
        [SerializeField] private LayerMask groundLayer;

        private bool _isGrounded;
        private bool _hasControl = true;

        private float _ySpeed;
        private Quaternion _targetRotation;

        private CameraController _cameraController;
        private Animator _animator;
        private CharacterController _characterController;

        private void Awake()
        {
            _cameraController = Camera.main.GetComponent<CameraController>();
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

            var moveInput = (new Vector3(h, 0, v)).normalized;

            var moveDir = _cameraController.PlanarRotation * moveInput;

            if (!_hasControl)
                return;

            GroundCheck();
            if (_isGrounded)
            {
                _ySpeed = -0.5f;
            }
            else
            {
                _ySpeed += Physics.gravity.y * Time.deltaTime;
            }

            var velocity = moveDir * moveSpeed;
            velocity.y = _ySpeed;

            _characterController.Move(velocity * Time.deltaTime);

            if (moveAmount > 0)
            {
                _targetRotation = Quaternion.LookRotation(moveDir);
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation,
                rotationSpeed * Time.deltaTime);

            _animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
        }

        private void GroundCheck()
        {
            _isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius,
                groundLayer);
        }

        public void SetControl(bool hasControl)
        {
            this._hasControl = hasControl;
            _characterController.enabled = hasControl;

            if (!hasControl)
            {
                _animator.SetFloat("moveAmount", 0f);
                _targetRotation = transform.rotation;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
        }

        public float RotationSpeed => rotationSpeed;
    }
}