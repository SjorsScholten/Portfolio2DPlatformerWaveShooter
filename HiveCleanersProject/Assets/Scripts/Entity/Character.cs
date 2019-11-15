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
    private PlayerControls PlayerInput;

    private void Awake() {
        _transform = GetComponent<Transform>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        PlayerInput = new PlayerControls();
        PlayerInput.Player.Move.performed += ctx => _moveDirection = ctx.ReadValue<float>();
        PlayerInput.Player.Jump.performed += ctx => _jumpPerformed = ctx.ReadValue<float>() > 0;
    }

    private void OnEnable() => PlayerInput.Player.Enable();
    private void OnDisable() => PlayerInput.Player.Disable();

    private void FixedUpdate() {
        GroundCheck();
        ProcessMove();
    }

    public void ProcessMove() {
        if (_moveDirection * _facingDirection < 0f) FlipSpriteDirection();
        _rigidbody2D.velocity = new Vector2(_moveDirection * moveSpeed, _rigidbody2D.velocity.y);
        if (_jumpPerformed && _isGrounded) ProcessJump();
    }

    private void FlipSpriteDirection() {
        _facingDirection *= -1;
        var scale = _transform.localScale;
        scale.x = _facingDirection;
        _transform.localScale = scale;
    }

    private void ProcessJump() {
        var force = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight) * _rigidbody2D.mass;
        _rigidbody2D.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        _jumpPerformed = false;
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