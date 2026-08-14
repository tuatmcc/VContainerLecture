using UnityEngine;
using VContainer;

namespace VContainerLecture.Play.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody _playerRigidbody;
        Animator _playerAnimator;
        IPlayerInput _playerInput; 
        IPlayerCamera _playerCamera;
        
        
        // フィールドインジェクションでも可
        // [Inject]
        // private IPlayerInput _playerInput;
        [Inject]
        public void Construct(IPlayerInput playerInput, IPlayerCamera playerCamera)
        {
            _playerInput = playerInput;
            _playerCamera = playerCamera; 
        }
        
        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
            _playerAnimator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            var move =  _playerInput.Move;
            var moveDir = _playerCamera.Forward * move.y +  _playerCamera.Right*move.x;
            moveDir = moveDir.normalized;
            
            var nextPosition = _playerRigidbody.position + moveDir*10.0f*Time.fixedDeltaTime; 
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
