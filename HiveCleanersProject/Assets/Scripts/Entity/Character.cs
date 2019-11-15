using Entity;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float footOffset = .4f;
    [SerializeField] private float groundCheckDistance = .2f;
    [SerializeField] private float jumpHeight = 3f;

    private float _moveDirection;
    private float _facingDirection = 1f;
    private bool _jumpPerformed;
    private bool _isGrounded;

    private Transform _transform;
    private Rigidbody2D _rigidbody2D;
    private GroundedCharacterController _controller;
    private PlayerControls PlayerInput;

    public float Mass => _rigidbody2D.mass;

    public Vector2 Velocity {
        get => _rigidbody2D.velocity;
        set => _rigidbody2D.velocity = value;
    }

    public Vector2 Impulse {
        set => _rigidbody2D.AddForce(value, ForceMode2D.Impulse);
    }

    private void Awake() {
        _transform = GetComponent<Transform>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        _controller = new GroundedCharacterController(this);
        
        PlayerInput = new PlayerControls();
        PlayerInput.Player.Move.performed += ctx => _moveDirection = ctx.ReadValue<float>();
        PlayerInput.Player.Jump.performed += ctx => _jumpPerformed = ctx.ReadValue<float>() > 0;
        PlayerInput.Player.Shoot.performed += ctx => ProcessShot();
    }

    private void OnEnable() => PlayerInput.Player.Enable();
    private void OnDisable() => PlayerInput.Player.Disable();

    private void ProcessShot() {
        Raycast(new Vector2(_facingDirection, 1f), Vector2.right * _facingDirection, 5f);
    }

    private void FixedUpdate() {
        GroundCheck();
        
        if (_moveDirection * _facingDirection < 0f) FlipSpriteDirection();
        
        ProcessMove();
        ProcessJump();
    }

    public void ProcessMove() => _controller.HandleMove(_moveDirection, moveSpeed);

    private void ProcessJump() {
        if (_jumpPerformed && _isGrounded) {
            _controller.HandleJump(jumpHeight);
            _jumpPerformed = false;
        }
    }

    private void FlipSpriteDirection() {
        _facingDirection *= -1;
        var scale = _transform.localScale;
        scale.x = _facingDirection;
        _transform.localScale = scale;
    }

    private void GroundCheck() {
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