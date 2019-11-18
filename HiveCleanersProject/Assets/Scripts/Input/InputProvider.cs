using System;
using UnityEngine;

namespace Input {
    [Serializable]
    public class InputProvider : MonoBehaviour {
        public float HorizontalAxisInput { get; private set; }
        public Vector2 MousePosition { get; private set; }

        
        public event Action OnJumpAction;
        public event Action OnAttackButton;

        
        private PlayerControls _playerControls;

        private void Awake() {
            _playerControls = new PlayerControls();

            _playerControls.Player.Move.performed += ctx => HorizontalAxisInput = ctx.ReadValue<float>();

            _playerControls.Player.Jump.performed += ctx => OnJumpAction?.Invoke();
            
            _playerControls.Player.AimDirection.performed += ctx => MousePosition = ctx.ReadValue<Vector2>();
            
            _playerControls.Player.Shoot.performed += ctx => OnAttackButton?.Invoke();
        }

        private void OnEnable() => _playerControls.Enable();
        private void OnDisable() => _playerControls.Disable();
    }
}