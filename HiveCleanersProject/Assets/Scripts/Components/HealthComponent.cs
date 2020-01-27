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
        /*
        [SerializeField] private bool regenerateHealth = false;
        [SerializeField] private float healthRegenerationStartTime = 2f;
        [SerializeField] private float healthRegenerationAmount = 10f;
        [SerializeField] private float healthRegenerationTick = 1f;

        [SerializeField] private TextMeshProUGUI text;

        private IEnumerator _regenerationCouroutine;

        //flags
        private bool _isRegenerating = false;

        private void Awake() {
            _currentHealth = initialHealth;
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
            while (_currentHealth < initialHealth) {
                ChangeCurrentHealth(healthRegenerationAmount);
                yield return new WaitForSeconds(healthRegenerationTick);
            }
        }

        */
    }
}