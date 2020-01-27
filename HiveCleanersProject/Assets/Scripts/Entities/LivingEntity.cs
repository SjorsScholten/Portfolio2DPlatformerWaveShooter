using UnityEngine;

namespace Entity {
    public class LivingEntity : MonoBehaviour, IDamageable {

        [SerializeField] private float initialHealth = 100f;
        [SerializeField] private bool destroyOnDeath = true;

        private float _currentHealth;

        private bool _isDead;

        protected virtual void Awake() {
            _currentHealth = initialHealth;
        }

        public void TakeHit(float damage = 0) {
            TakeDamage(damage);
        }

        public void TakeDamage(float damage) {
            if (damage < 0 || _isDead) return;
            _currentHealth -= damage;
            if (_currentHealth <= 0)  Die();
        }

        public void Heal(float value) {
            if (value < 0 || _isDead) return;
            var newHealth = _currentHealth + value;
            _currentHealth = newHealth > initialHealth ? initialHealth : newHealth;
        }

        private void Die() {
            if (_isDead) return;
            _isDead = true;
            if(destroyOnDeath) Destroy(this.gameObject);
        }
    }

    public interface IDamageable {
        void TakeHit(float damage);
    }
}