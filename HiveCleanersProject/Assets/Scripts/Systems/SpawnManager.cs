using System;
using System.Collections.Generic;
using Components.Spawn;
using Components.Timer;
using UnityEngine;
using Util.ExtensionMethods;
using Random = UnityEngine.Random;

namespace Systems {
    public class SpawnManager : MonoBehaviour {
        [SerializeField] private int allowedAmount = 20;
        [SerializeField] private float initialSpawnDelay = 12f;
        [SerializeField] private float concurrentSpawnDelay = 4f;
        [SerializeField] private float decayFactor = 0.3f;

        [SerializeField] private GameObject prefab = null;

        private float _spawnDelay;
        private float _timeLastSpawn;
        private int _lastSpawnAtIndex;
    
        [SerializeField] private Timer timer = new Timer();
        private readonly List<SpawnComponent> _spawnComponents = new List<SpawnComponent>();

        private void Awake() {
            _spawnComponents.AddRange(FindObjectsOfType<SpawnComponent>());
            
            if (_spawnComponents.IsEmpty() || !prefab) {
                Debug.LogError("there is no spawnComponent in scene, deactivating object");
                this.gameObject.SetActive(false);
            }

            timer.OnTimerEnd += ProcessSpawnEntity;
        }

        private void Start() {
            timer.InitialTime = SpawnDelay + initialSpawnDelay;
            timer.Start();
        }
        
        private void Update() => timer.Update(Time.deltaTime);

        public void ProcessSpawnEntity() {
            if (_spawnComponents.IsEmpty()) {
                Debug.Log("there are no spawn components");
                return;
            }

            if (timer.IsAwake) {
                Debug.Log("cooldown hasnt ended");
                return;
            }

            if (transform.childCount - 1 >= allowedAmount) {
                Debug.Log("there are too many instances");
                return;
            }
            
            GetSpawn().SpawnInstance(Instantiate(prefab, transform));
            
            timer.InitialTime = SpawnDelay;
            timer.Start();
        }
    
        private float SpawnDelay => concurrentSpawnDelay * Mathf.Pow(1 - decayFactor / 60, Time.time);

        private SpawnComponent GetSpawn() {
            var randomIndex = 0;
            if (_spawnComponents.Count > 1) {
                do randomIndex = Random.Range(0, _spawnComponents.Count);
                while (randomIndex == _lastSpawnAtIndex);
            }
            _lastSpawnAtIndex = randomIndex;
            return _spawnComponents[randomIndex];
        }
    }
}