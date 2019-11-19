using Input;
using UnityEngine;

namespace Components.Attack {
    [RequireComponent(typeof(InputProvider))]
    public class AttackComponent : MonoBehaviour {
        [SerializeField] private ScriptableObjects.Attack attack;
        
        private InputProvider _inputProvider;
        private Transform _transform;

        private void Awake() {
            _inputProvider = GetComponent<InputProvider>();
            _transform = GetComponent<Transform>();
        }

        private void OnEnable() => _inputProvider.OnAttackButton += OnAttack;
        private void OnDisable() => _inputProvider.OnAttackButton -= OnAttack;
        

        public void OnAttack() {
            var direction = (Vector2)Camera.main.ScreenToWorldPoint(_inputProvider.MousePosition) - (Vector2)_transform.position;
            attack.HandleAttack(_transform.position, direction, 8);
        }
    }
}
