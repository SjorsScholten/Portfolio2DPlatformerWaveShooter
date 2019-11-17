using System;
using UnityEngine;

namespace Components.Movement.Behaviours {
    public abstract class MovementBehaviour{
        public bool UseFixedUpdate { get; }

        public abstract void HandleMove(Rigidbody2D rigidbody2D, Vector2 direction, float speed);
        public abstract void HandleJump(Rigidbody2D rigidbody2D, float jumpHeight);
    }
}