using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Components {
    public class HealthComponent : MonoBehaviour {
        [SerializeField] private float initialHealth = 100f;
    
        [SerializeField] private bool regenerateHealth = false;
        [SerializeField] private float healthRegenerationStartTime = 2f;
        [SerializeField] private float healthRegenerationAmount = 10f;
        [SerializeField] private float healthRegenerationTick = 1f;

        [SerializeField] private TextMeshProUGUI text;

        public UnityEvent onDamage = new UnityEvent();
        public UnityEvent onDeath = new UnityEvent();

        private float _currentHealth;
        private IEnumerator _regenerationCouroutine;

        //flags
        private bool _isDead = false;
        private bool _isRegenerating = false;

        private TargetComponent _targetComponent;

        public event Action<float> OnHealthPercentChanged;

        private void Awake() {
            _targetComponent = GetComponent<TargetComponent>();
            CurrentHealth = initialHealth;
        }

        private void OnEnable() {
            if (_targetComponent) _targetComponent.OnHit.AddListener(TakeDamage);
        }

        private void OnDisable() {
            if(_targetComponent) _targetComponent.OnHit.RemoveListener(TakeDamage);
        }

        public void TakeDamage(float damage) {
            if (damage < 0 || _isDead) return;

            StopHealthRegenerating();
        
            CurrentHealth -= damage;
        
            if (CurrentHealth <= 0) {
                Die();
                return;
            }

            if (regenerateHealth && !_isDead)
                StartHealthRegeneration();
            
            onDamage?.Invoke();
        }

        public void Heal(float value) {
            if (value < 0) return;
        
            if (CurrentHealth + value > initialHealth) CurrentHealth = initialHealth;
            else CurrentHealth += value;
        }

        private void Die() {
            if (_isDead) return;
            _isDead = true;
            onDeath.Invoke();
            Destroy(this.gameObject);
        }

        private void StartHealthRegeneration() {
            if (_isRegenerating) StopHealthRegenerating();
        
            _regenerationCouroutine = HealthRegenerating();
        
            StartCoroutine(_regenerationCouroutine);
            _isRegenerating = true;
        }

        private void StopHealthRegenerating() {
            if (!_isRegenerating) return;
        
            StopCoroutine(_regenerationCouroutine);
            _isRegenerating = false;
        }

        private IEnumerator HealthRegenerating() {
            yield return new WaitForSeconds(healthRegenerationStartTime);
            while (CurrentHealth < initialHealth) {
                CurrentHealth += healthRegenerationAmount;
                yield return new WaitForSeconds(healthRegenerationTick);
            }
        }

    
        public float CurrentHealth {
            get => _currentHealth;
            private set {
                _currentHealth = value;
                if(text) text.text = _currentHealth.ToString(CultureInfo.InvariantCulture);
                OnHealthPercentChanged?.Invoke(HealthPercent);
            }
        }

        public float HealthPercent => CurrentHealth / initialHealth;
    }
}