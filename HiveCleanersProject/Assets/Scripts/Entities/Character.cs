using Components;
using Components.Attack;
using Components.Attack.ScriptableObjects;
using Components.Movement;
using Input;
using UnityEngine;

namespace Entity {
    [RequireComponent(typeof(InputProvider))]
    [RequireComponent(typeof(MovementComponent))]
    
    public class Character : LivingEntity {
        private float _facingDirection = 1f;

        private Weapon _currentWeapon;

        private InputProvider _inputProvider;
        private Transform _transform;
        
        private MovementComponent _movementComponent;

        protected override void Awake() { base.Awake();
            _inputProvider = GetComponent<InputProvider>();
            _transform = GetComponent<Transform>();
            _movementComponent = GetComponent<MovementComponent>();
        }
        
        private void OnEnable() {
            _inputProvider.OnJumpAction += ProcessJump;
            _inputProvider.OnAttackAction += ProcessAttack;
        }

        private void OnDisable() {
            _inputProvider.OnJumpAction -= ProcessJump;
            _inputProvider.OnAttackAction -= ProcessAttack;
        }

        private void FixedUpdate() {
            ProcessMove();
        }
        
        private void ProcessMove() {
            var horizontalAxis = _inputProvider.HorizontalAxisInput;
            _movementComponent.Move(horizontalAxis);
        }

        private void ProcessJump() => _movementComponent.SetJumpRequest();

        private void ProcessAttack() {
            if(_currentWeapon == null) return;
            _currentWeapon.Fire();
        }
        
        private void FlipSpriteDirection() {
            _facingDirection *= -1;
            var scale = _transform.localScale;
            scale.x = _facingDirection;
            _transform.localScale = scale;
        }
    }
}