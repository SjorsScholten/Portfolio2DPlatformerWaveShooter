using System;
using UnityEngine;

namespace Input {
    public class EnemyInputProvider : MonoBehaviour, IInputProvider {
        public float HorizontalAxisInput { get; private set; } = 0f;
        public float VerticalAxisInput { get; private set; } = 0f;
        public Vector2 LookDirection { get; private set; } = Vector2.zero;

        public event Action OnJumpAction;
        public event Action OnAttackAction;

        private const float AttackDistance = 1.5f;
        private float _distanceToTarget;
        private float _lastAttack;
        
        private Transform _target;
        private Transform _transform;

        private void Awake() {
            _transform = GetComponent<Transform>();
            _target = GameObject.FindWithTag("Player").transform;
        }

        private void Update() {
            if (!_target) return;
            
            _distanceToTarget = Vector2.Distance(_target.position, _transform.position);
            
            if (_distanceToTarget < AttackDistance) {
                if(Time.time > _lastAttack + 1) ProcessAttack();
            }
            else ProcessMove();
        }

        private void ProcessAttack() {
            HorizontalAxisInput = 0;
            LookDirection = _target.position;
            OnAttackAction?.Invoke();
            _lastAttack = Time.time;
        }

        private void ProcessMove() {
            Vector2 direction = _target.position - _transform.position;
            direction.Normalize();
            HorizontalAxisInput = direction.x;
        }
    }
}