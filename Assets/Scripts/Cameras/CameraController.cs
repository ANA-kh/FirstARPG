using UnityEngine;
namespace FirstARPG.Cameras
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;

        [SerializeField] private float rotationSpeed = 2f;
        [SerializeField] private float distance = 5;

        [SerializeField] private float minVerticalAngle = -10;
        [SerializeField] private float maxVerticalAngle = 45;

        [SerializeField] private Vector2 framingOffset;

        [SerializeField] private bool invertX;
        [SerializeField] private bool invertY;


        private float _rotationX;
        private float _rotationY;

        private float _invertXVal;
        private float _invertYVal;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            _invertXVal =  (invertX) ? -1 : 1;
            _invertYVal =  (invertY) ? -1 : 1;

            _rotationX += Input.GetAxis("Mouse Y") * _invertYVal * rotationSpeed;
            _rotationX = Mathf.Clamp(_rotationX, minVerticalAngle, maxVerticalAngle);

            _rotationY += Input.GetAxis("Mouse X") * _invertXVal * rotationSpeed;

            var targetRotation = Quaternion.Euler(_rotationX, _rotationY, 0);

            var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

            transform.position = focusPosition - targetRotation * new Vector3(0, 0, distance);
            transform.rotation = targetRotation;
        }

        public Quaternion PlanarRotation => Quaternion.Euler(0, _rotationY, 0);
    }

}