using Components.Movement.Behaviours;
using Input;
using UnityEngine;

namespace Components.Movement {
    [RequireComponent(typeof(InputProvider))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementComponent : MonoBehaviour {
        [SerializeField] private float speed = 5f;
        
        [Header("Jump Attributes")]
        [SerializeField] private float jumpHeight = 3f;
        [SerializeField] private float footOffset = .4f;
        [SerializeField] private float groundCheckDistance = .2f;

        private bool _jumpRequest;
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

        private void OnEnable() {
            _inputProvider.OnJumpAction += SetJumpRequest;
        }

        private void OnDisable() {
            _inputProvider.OnJumpAction -= SetJumpRequest;
        }

        private void FixedUpdate() {
            CheckPhysics();
            ProcessMove(_inputProvider.HorizontalAxisInput);
            ProcessJump();
        }

        
        
        private void CheckPhysics() {
            var leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundCheckDistance);
            var rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundCheckDistance);
            _isGrounded = leftCheck || rightCheck;
        }

        public void ProcessMove(float horizontalAxisInput, float verticalAxisInput = 0) {
            var direction2D = new Vector2(horizontalAxisInput, verticalAxisInput);
            behaviour.HandleMove(_rigidbody2D, direction2D, speed);
        }

        public void SetJumpRequest() => _jumpRequest = true;

        private void ProcessJump() {
            if(_jumpRequest && _isGrounded) behaviour.HandleJump(_rigidbody2D, jumpHeight);
            _jumpRequest = false;
        }

        
        
        private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length) {
            var pos = (Vector2)_transform.position;
            var hit = Physics2D.Raycast(pos + offset, rayDirection, length);
            var color = hit ? Color.red : Color.green;
            Debug.DrawRay(pos + offset, rayDirection * length, color);
            return hit;
        }
    }
}