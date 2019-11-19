using Components.Movement;
using UnityEngine;

namespace Entity {
    [RequireComponent(typeof(MovementComponent))]
    public class Character : MonoBehaviour {
        private float _facingDirection = 1f;

        private Transform _transform;

        private void Awake() {
            _transform = GetComponent<Transform>();
        }

        private void FlipSpriteDirection() {
            _facingDirection *= -1;
            var scale = _transform.localScale;
            scale.x = _facingDirection;
            _transform.localScale = scale;
        }
    }
}