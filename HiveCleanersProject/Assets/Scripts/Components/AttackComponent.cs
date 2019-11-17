using Input;
using UnityEngine;

namespace Components {
    [RequireComponent(typeof(InputProvider))]
    public class AttackComponent : MonoBehaviour {

        private InputProvider _inputProvider;

        private void Awake() {
            _inputProvider = GetComponent<InputProvider>();
        }

        public void ProcessAttack(IButtonInput attackButtonInput) {
            
        }
        
        private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length) {
            var pos = (Vector2)transform.position;
            var hit = Physics2D.Raycast(pos + offset, rayDirection, length);
            var color = hit ? Color.red : Color.green;
            Debug.DrawRay(pos + offset, rayDirection * length, color);
            return hit;
        }
    }
}
