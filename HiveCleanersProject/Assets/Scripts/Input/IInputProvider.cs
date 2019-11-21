using System;
using UnityEngine;

namespace Input {
    public interface IInputProvider {
        float HorizontalAxisInput { get; }
        float VerticalAxisInput { get; }
        Vector2 LookDirection { get; }

        event Action OnJumpAction;
        event Action OnAttackAction;
    }
}