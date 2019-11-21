using System;
using UnityEngine;

namespace Input {
    public class EnemyInputProvider : MonoBehaviour, IInputProvider {
        public float HorizontalAxisInput { get; private set; } = 0f;
        public float VerticalAxisInput { get; private set; } = 0f;
        public Vector2 LookDirection { get; private set; } = Vector2.zero;
        
        public event Action OnJumpAction;
        public event Action OnAttackAction;
    }
}