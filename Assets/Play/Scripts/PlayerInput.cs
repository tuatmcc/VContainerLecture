using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Play.Scripts
{
    public class PlayerInput : IPlayerInput, IDisposable, ITickable
    {
        private readonly  GameInputs _gameinputs;

        private bool jumpPressed;

        public bool JumpPressed
        {
            get
            {
                bool isJumpPressed = jumpPressed;
                jumpPressed = false;
                return isJumpPressed;
            }    
        }
        
        public Vector2 Move => _gameinputs.Player.Move.ReadValue<Vector2>();
        public Vector2 Look => _gameinputs.Player.Look.ReadValue<Vector2>();

        public PlayerInput()
        {
            _gameinputs = new GameInputs();
            _gameinputs.Player.Jump.performed += _ => jumpPressed = true;
            _gameinputs.Player.Enable();
        }

        public void Tick()
        {
        }

        public void Dispose()
        {
            _gameinputs.Player.Disable();
        }
    }
}