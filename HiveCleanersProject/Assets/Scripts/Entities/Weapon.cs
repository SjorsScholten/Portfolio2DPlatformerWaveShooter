using System.Collections;
using Components.Attack.ScriptableObjects;
using UnityEngine;

namespace Entity {
    public class Weapon : MonoBehaviour {
        private float _damage;
        private float _range;
        private float _attackSpeed;
        private int _seriesLength;
        private float _seriesCooldown;

        private Attack _attackType;

        private ParticleSystem _shootParticles;
        private ParticleSystem _hitParticleSystem;

        private float _timeLastAttack;
        private int _attacksBeforeCooldown;
        private WaitForSeconds _cooldownTime;
        
        private bool _isReloading;

        private void Awake() {
            _cooldownTime = new WaitForSeconds(_seriesCooldown);
            _attacksBeforeCooldown = _seriesLength;
        }

        public void Fire() {
            if (_isReloading) return;
            
            float currentTime = Time.time;
            if (currentTime< _timeLastAttack + _attackSpeed) return;
            _timeLastAttack = currentTime;
            
            //_attackType.HandleAttack();
            
            _attacksBeforeCooldown--;
            if (_attacksBeforeCooldown <= 0) StartCoroutine(Reload());
        }

        IEnumerator Reload() {
            _isReloading = true;
            yield return _cooldownTime;
            _attacksBeforeCooldown = _seriesLength;
            _isReloading = false;
        }
    }
}