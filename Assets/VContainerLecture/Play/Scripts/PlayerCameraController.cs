using UnityEngine;
using VContainer;

namespace VContainerLecture.Play.Scripts
{
    public class PlayerCameraController : MonoBehaviour, IPlayerCamera
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private Transform followTarget;

        private IPlayerInput _playerInput;
        private PlaySettings _playSettings;
        private float _yaw;
        private float _pitch;
        private float _currentDistance;
        private float _distanceVelocity;

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
        public void Construct(IPlayerInput playerInputs, PlaySettings playSettings)
        {
            _playerInput = playerInputs;
            _playSettings = playSettings;
        }

        private void Start()
        {
            _currentDistance = _playSettings.DefaultDistance;
        }

        private void LateUpdate()
        {
            var look = _playerInput.Look;
            _yaw += look.x * _playSettings.LookSensitivity;
            _pitch += look.y * _playSettings.LookSensitivity;
            _pitch = Mathf.Clamp(_pitch, _playSettings.MinPitch, _playSettings.MaxPitch);

            var pivot = followTarget.position + Vector3.up * _playSettings.TargetHeight;
            transform.position = pivot;
            var rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            cameraRoot.rotation = rotation;
            
            var desiredDirection = rotation * Vector3.back;
            var desiredDistance = _playSettings.DefaultDistance;

            if (Physics.SphereCast(
                    pivot,
                    _playSettings.CameraRadius,
                    desiredDirection,
                    out var hit,
                    _playSettings.DefaultDistance,
                    _playSettings.CollisionLayers,
                    QueryTriggerInteraction.Ignore))
            {
                desiredDistance = Mathf.Clamp(hit.distance - _playSettings.CameraRadius, _playSettings.MinDistance,
                    _playSettings.DefaultDistance);
            }
            
            var smoothTime = desiredDistance < _currentDistance ? _playSettings.CollisionInSmooth : _playSettings.CollisionOutSmooth; 
            _currentDistance = Mathf.SmoothDamp(
                _currentDistance,
                desiredDistance,
                ref _distanceVelocity,
                smoothTime);
            cameraTransform.position = pivot + desiredDirection * _currentDistance;
            cameraTransform.rotation = rotation;
        }
    }
}
