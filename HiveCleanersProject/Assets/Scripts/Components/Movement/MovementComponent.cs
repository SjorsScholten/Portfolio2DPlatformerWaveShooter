using Components.Movement.Behaviours;
using Input;
using UnityEngine;

namespace Components.Movement {
    [RequireComponent(typeof(InputProvider))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementComponent : MonoBehaviour {
        [SerializeField] private float speed;
        
        [Header("Jump Attributes")]
        [SerializeField] private float jumpHeight;
        [SerializeField] private float footOffset;
        [SerializeField] private float groundCheckDistance;

        private bool _isGrounded;
    
        public MovementBehaviour behaviour = new Grounded2DMovementBehaviour();
        
        private InputProvider _inputProvider;
        private Transform _transform;
        private Rigidbody2D _rigidbody2D;

        private void Awake() {
            _inputProvider = GetComponent<InputProvider>();
            _transform = GetComponent<Transform>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            CheckPhysics();
            ProcessMove(_inputProvider.HorizontalAxisInput, new CHorizontalAxisInput());
        }

        public void ProcessMove(IAxisInput horizontlaAxisInput, IAxisInput verticalAxisInput) {
            var direction2D = new Vector2(horizontlaAxisInput.GetAxis(), verticalAxisInput.GetAxis());
            behaviour.HandleMove(_rigidbody2D, direction2D, speed);
        }

        public void ProcessJump(IButtonInput jumpButtonInput) {
            if(jumpButtonInput.GetButtonDown() && _isGrounded)
                behaviour.HandleJump(_rigidbody2D, jumpHeight);
        }

        private void CheckPhysics() {
            var leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundCheckDistance);
            var rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundCheckDistance);
            _isGrounded = leftCheck || rightCheck;
        }

        private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length) {
            var pos = (Vector2)transform.position;
            var hit = Physics2D.Raycast(pos + offset, rayDirection, length);
            var color = hit ? Color.red : Color.green;
            Debug.DrawRay(pos + offset, rayDirection * length, color);
            return hit;
        }
    }
}