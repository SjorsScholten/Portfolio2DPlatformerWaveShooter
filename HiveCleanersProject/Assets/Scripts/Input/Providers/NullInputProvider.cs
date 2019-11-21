using System;
using UnityEngine;

namespace Input {
    public class NullInputProvider : IInputProvider {
        public float HorizontalAxisInput { get; } = 0;
        public float VerticalAxisInput { get; } = 0;
        public Vector2 LookDirection { get; } = Vector2.zero;
        
        public event Action OnJumpAction;
        public event Action OnAttackAction;
    }
}