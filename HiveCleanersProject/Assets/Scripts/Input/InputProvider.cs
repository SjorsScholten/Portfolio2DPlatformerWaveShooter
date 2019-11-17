using System;
using UnityEngine;

namespace Input {
    [Serializable]
    public class InputProvider : MonoBehaviour {
        private readonly CHorizontalAxisInput _horizontalAxisInput = new CHorizontalAxisInput();
        public IAxisInput HorizontalAxisInput => _horizontalAxisInput;

        private PlayerControls _playerControls;

        private void Awake() {
            _playerControls = new PlayerControls();

            _playerControls.Player.Move.performed += ctx => _horizontalAxisInput.Axis = ctx.ReadValue<float>();
        }

        private void OnEnable() => _playerControls.Enable();
        private void OnDisable() => _playerControls.Disable();
    }

    internal class CHorizontalAxisInput : IAxisInput {
        public float Axis;
        public float GetAxis() => Axis;
    }
}