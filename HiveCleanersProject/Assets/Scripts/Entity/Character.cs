using Entity;
using Input;
using UnityEngine;
using Util.StateMachine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float footOffset = .4f;
    [SerializeField] private float groundCheckDistance = .2f;
    [SerializeField] private float jumpHeight = 3f;

    private float _moveDirection;
    private float _facingDirection = 1f;
    private bool _JumpPerformed;
    private bool _isGrounded;

    private Transform _transform;
    private Rigidbody2D _rigidbody2D;
    private PlayerControls PlayerInput;

    private void Awake() {
        _transform = GetComponent<Transform>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        PlayerInput = new PlayerControls();
        PlayerInput.Player.Move.performed += ctx => _moveDirection = ctx.ReadValue<float>();
        PlayerInput.Player.Jump.performed += ctx => _JumpPerformed = ctx.ReadValue<float>() > 0;
    }

    private void OnEnable() => PlayerInput.Player.Enable();
    private void OnDisable() => PlayerInput.Player.Disable();

    private void FixedUpdate() {
        _isGrounded = GroundCheck();
        ProcessMove();
    }

    public void ProcessMove() {
        if (_moveDirection * _facingDirection < 0f) FlipSpriteDirection();
        _rigidbody2D.velocity = new Vector2(_moveDirection * moveSpeed, _rigidbody2D.velocity.y);

        if (_JumpPerformed && _isGrounded) ProcessJump();
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
        _JumpPerformed = false;
    }

    private bool GroundCheck() {
            var position = (Vector2)_transform.position;
            var leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundCheckDistance);
            var rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundCheckDistance);
            return leftCheck || rightCheck;
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        //Record the player's position
        Vector2 pos = transform.position;

        //Send out the desired raycasr and record the result
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length);

        //...determine the color based on if the raycast hit...
        Color color = hit ? Color.red : Color.green;
        //...and draw the ray in the scene view
        Debug.DrawRay(pos + offset, rayDirection * length, color);
            
        //Return the results of the raycast
        return hit;
    }
}