using UnityEngine;

namespace Entity {
    public class GroundedCharacterController {
        private readonly Character _character;

        public GroundedCharacterController(Character character) {
            this._character = character;
        }

        public void HandleMove(float direction, float speed) {
            _character.Velocity = new Vector2(direction * speed, _character.Velocity.y);
        }

        public void HandleJump(float jumpHeight) {
            var force = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight) * _character.Mass;
            _character.Impulse = Vector2.up * force;
        }
    }
}