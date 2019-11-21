using Input;
using UnityEngine;

namespace Components.Attack {
    [RequireComponent(typeof(InputProvider))]
    public class AttackComponent : MonoBehaviour {
        [SerializeField] private ScriptableObjects.Attack attack = null;
        
        private InputProvider _inputProvider;
        private Transform _transform;

        private void Awake() {
            _inputProvider = GetComponent<InputProvider>();
            _transform = GetComponent<Transform>();
        }

        private void OnEnable() => _inputProvider.OnAttackAction += OnAttack;
        private void OnDisable() => _inputProvider.OnAttackAction -= OnAttack;
        

        public void OnAttack() {
            var position = _transform.position;
            var direction = (Vector2)Camera.main.ScreenToWorldPoint(_inputProvider.MousePosition) - (Vector2)position;
            attack.HandleAttack(position, direction, 8);
        }
    }
}
