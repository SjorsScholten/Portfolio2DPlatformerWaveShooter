using System;
using UnityEngine;

namespace Components.Movement.Behaviours {
    public class Grounded2DMovementBehaviour : MovementBehaviour {
    
        public override void HandleMove(Rigidbody2D rigidbody2D, Vector2 direction, float speed) {
            var horizontalSpeed = direction.x * speed;
            rigidbody2D.velocity = new Vector2(horizontalSpeed, rigidbody2D.velocity.y);
        }

        public override void HandleJump(Rigidbody2D rigidbody2D, float jumpHeight) {
            var force = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight) * rigidbody2D.mass;
            rigidbody2D.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
    }
}