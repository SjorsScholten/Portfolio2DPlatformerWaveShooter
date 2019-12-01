using Input;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Attack {
    [RequireComponent(typeof(InputProvider))]
    public class AttackComponent : MonoBehaviour {
        [SerializeField] private ScriptableObjects.Attack attack = null;
        [SerializeField] private Vector2 offset = new Vector2(0f,1f);
        
        public UnityEvent OnAttack = new UnityEvent();
        
        private InputProvider _inputProvider;
        private Transform _transform;

        private void Awake() {
            _inputProvider = GetComponent<InputProvider>();
            _inputProvider.OnAttackAction += ProcessAttack;
            
            _transform = GetComponent<Transform>();
        }

        public void ProcessAttack() {
            attack.HandleAttack(Position, Direction, gameObject.layer);
            OnAttack?.Invoke();
        }

        public Vector2 Direction => (_inputProvider.LookDirection - Position).normalized;
        public Vector2 Position => (Vector2)_transform.position + offset;
    }
}
