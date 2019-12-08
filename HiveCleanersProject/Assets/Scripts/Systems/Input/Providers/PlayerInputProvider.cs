using System;
using UnityEngine;
using Util;

namespace Input {
    public class PlayerInputProvider : Singleton<PlayerInputProvider>, IInputProvider {
        public float HorizontalAxisInput { get; private set; }
        public float VerticalAxisInput { get; private set; }
        public Vector2 LookDirection { get; private set; }
        public event Action OnJumpAction;
        public event Action OnAttackAction;

        private PlayerControls _playerControls;

        private void Awake() {
            _playerControls = new PlayerControls();
            
            _playerControls.Player.Move.performed += ctx => HorizontalAxisInput = ctx.ReadValue<float>();

            _playerControls.Player.Jump.performed += ctx => OnJumpAction?.Invoke();
            
            _playerControls.Player.AimDirection.performed += ctx => LookDirection = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
            
            _playerControls.Player.Shoot.performed += ctx => OnAttackAction?.Invoke();
        }

        private void OnEnable() => _playerControls.Enable();
        private void OnDisable() => _playerControls.Disable();
    }
}