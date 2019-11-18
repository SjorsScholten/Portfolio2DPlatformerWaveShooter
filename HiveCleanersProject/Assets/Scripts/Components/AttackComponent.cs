using System;
using Input;
using UnityEngine;

namespace Components {
    [RequireComponent(typeof(InputProvider))]
    public class AttackComponent : MonoBehaviour {
        private InputProvider _inputProvider;
        private Transform _transform;

        private void Awake() {
            _inputProvider = GetComponent<InputProvider>();
            _transform = GetComponent<Transform>();
        }

        private void OnEnable() {
            _inputProvider.OnAttackButton += ProcessAttack;
        }

        private void OnDisable() {
            _inputProvider.OnAttackButton -= ProcessAttack;
        }

        public void ProcessAttack() {
            var direction = (Vector2)Camera.main.ScreenToWorldPoint(_inputProvider.MousePosition);
            var hit = Raycast(Vector2.up, direction, 10f);
            if (!hit) return;
            var target = hit.transform.GetComponent<TargetComponent>();
            if (target) target.ProcessTakeHit();
        }
        
        private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length) {
            var pos = (Vector2)_transform.position;
            var hit = Physics2D.Raycast(pos + offset, rayDirection, length, ~(1<<8));
            var color = hit ? Color.red : Color.green;
            Debug.DrawRay(pos + offset, rayDirection * length, color);
            return hit;
        }
    }
}
