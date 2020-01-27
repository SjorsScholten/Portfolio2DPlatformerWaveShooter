using Input;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Attack {
    [RequireComponent(typeof(InputProvider))]
    public class AttackComponent : MonoBehaviour {
        [SerializeField] private ScriptableObjects.Attack attack = null;
        [SerializeField] private Vector2 offset = new Vector2(0f,1f);

        public void Attack(Vector2 position, Vector2 direction) {
            attack.HandleAttack(position, direction, gameObject.layer);
        }
    }
}
