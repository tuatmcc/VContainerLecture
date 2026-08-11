using UnityEngine;
using VContainer;

namespace Play.Scripts
{
    public class PlayerCameraController : MonoBehaviour, IPlayerCamera
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private Transform followTarget;

        private IPlayerInput _playerInput;
        private float _yaw;
        private float _pitch;

        public Vector3 Forward 
        {
            get
            {
                var forward = cameraTransform.forward;
                forward.y = 0;
                return forward.normalized;
            }
        }

        public Vector3 Right
        {
            get
            {
                var right = cameraTransform.right;
                right.y = 0;
                return right.normalized;
            }
        }
        [Inject]
        public void Construct(IPlayerInput playerInputs)
        {
            _playerInput = playerInputs;
        }

        private void LateUpdate()
        {
            var look = _playerInput.Look;
            _yaw += look.x * 0.1f;
            _pitch += look.y * 0.1f;
            _pitch = Mathf.Clamp(_pitch, -90, 90);
            cameraRoot.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            
            transform.position = followTarget.position;
        }
    }
}