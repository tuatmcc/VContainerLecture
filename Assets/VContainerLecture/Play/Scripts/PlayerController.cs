using UnityEngine;
using VContainer;

namespace VContainerLecture.Play.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody _playerRigidbody;
        Animator _playerAnimator;
        CapsuleCollider _playerCollider;
        IPlayerInput _playerInput; 
        IPlayerCamera _playerCamera;
        PlaySettings  _playSettings;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        
        // フィールドインジェクションでも可
        // [Inject]
        // private IPlayerInput _playerInput;
        [Inject]
        public void Construct(IPlayerInput playerInput, IPlayerCamera playerCamera, PlaySettings  settings)
        {
            _playerInput = playerInput;
            _playerCamera = playerCamera; 
            _playSettings = settings;
        }
        
        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
            _playerAnimator = GetComponent<Animator>();
            _playerCollider = GetComponent<CapsuleCollider>();

        }

        private void FixedUpdate()
        {
            var center = _playerRigidbody.position + _playerCollider.center;
            var radius = _playerCollider.radius;
            var height = _playerCollider.height;
            var halfHeight = Mathf.Max(height * 0.5f - radius, 0.0f);
            var bottom = center + Vector3.down * halfHeight;
            var top = center + Vector3.up * halfHeight;
            
            var move =  _playerInput.Move;
            var moveDir = _playerCamera.Forward * move.y +  _playerCamera.Right*move.x;
            moveDir = moveDir.normalized;

            var moveDistance =  _playSettings.MoveSpeed * Time.fixedDeltaTime;
            var nextPosition = _playerRigidbody.position;
            
            
            
            // var skinWidth = 0.02f;

            if (moveDir.sqrMagnitude > 0.0001f)
            {
                if (Physics.CapsuleCast(
                        bottom,
                        top,
                        radius,
                        moveDir,
                        out var hit,
                        moveDistance,
                        _playSettings.CollisionLayers,
                        QueryTriggerInteraction.Ignore
                    ))
                {
                    var moveToWall = Mathf.Max(hit.distance - _playSettings.SkinWidth, 0.0f);
                    nextPosition += moveDir * moveToWall;

                    var remainingDistance = moveDistance - moveToWall;
                    var slideDir = Vector3.ProjectOnPlane(moveDir, hit.normal).normalized;

                    if (slideDir.sqrMagnitude > 0.0001f)
                    {
                        var offset = nextPosition - _playerRigidbody.position;
                        var slideBottom = bottom + offset;
                        var slideTop = top + offset;

                        if (Physics.CapsuleCast(
                                slideBottom,
                                slideTop,
                                radius,
                                slideDir,
                                out var slideHit,
                                remainingDistance,
                                _playSettings.CollisionLayers,
                                QueryTriggerInteraction.Ignore
                            ))
                        {
                            nextPosition += slideDir * Mathf.Max(slideHit.distance - _playSettings.SkinWidth, 0.0f);
                        }
                        else
                        {
                            nextPosition += slideDir * remainingDistance;
                        }
                    }
                }
                else
                {
                    nextPosition += moveDir * moveDistance;
                }
            }
            var actualMove = nextPosition - _playerRigidbody.position;
            actualMove.y = 0.0f;
            var actualSpeed = actualMove.magnitude/Time.fixedDeltaTime;
            var speedRate = Mathf.Clamp01(actualSpeed/ _playSettings.MoveSpeed);
            _playerAnimator.SetFloat(SpeedHash, speedRate, 0.1f, Time.fixedDeltaTime);
            
            _playerRigidbody.MovePosition(nextPosition);

            if (moveDir.magnitude > 0.1f)
            {
                var targetRotation = Quaternion.LookRotation(moveDir,  Vector3.up);
                _playerRigidbody.MoveRotation(
                    Quaternion.Slerp(_playerRigidbody.rotation, targetRotation, Time.fixedDeltaTime*10.0f));
            }
            
            if (_playerInput.JumpPressed)
            {
                _playerRigidbody.AddForce(Vector3.up, ForceMode.VelocityChange);
                
            }
        }
    }
}
