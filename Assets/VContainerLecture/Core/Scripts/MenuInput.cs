using System;
using UnityEngine.InputSystem;

namespace VContainerLecture.Core.Scripts
{
    public class MenuInput : IMenuInput, IDisposable
    {
        private readonly GameInputs gameInputs;
        private readonly InputActionMap menuActionMap;
        private bool submitPressed;

        public bool SubmitPressed
        {
            get
            {
                bool wasPressed = submitPressed;
                submitPressed = false;
                return wasPressed;
            }
        }

        public MenuInput()
        {
            gameInputs = new GameInputs();
            menuActionMap = gameInputs.asset.FindActionMap("Menu", throwIfNotFound: true);
            menuActionMap.FindAction("Submit", throwIfNotFound: true).performed += _ => submitPressed = true;
            menuActionMap.Enable();
        }

        public void Dispose()
        {
            menuActionMap.Disable();
            gameInputs.Dispose();
        }
    }
}
