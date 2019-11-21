using System.Collections.Generic;
using Components;
using UnityEngine;
using Util;

public class SpawnManager : MonoBehaviour {
    //[SerializeField] private int allowedAmount = 20;
    [SerializeField] private float initialSpawnDelay = 12f;
    [SerializeField] private float concurrentSpawnDelay = 5f;
    [SerializeField] private float decayFactor = 0.2f;

    private float _spawnDelay;
    private float _timeLastSpawn;
    private int _lastSpawnAtIndex;
    
    private readonly List<SpawnComponent> _spawnComponents = new List<SpawnComponent>();
    private readonly Queue<GameObject> _objects = new Queue<GameObject>();

    private void Awake() {

        if (_spawnComponents.Count == 0) {
            Debug.LogError("there is no spawnComponent in scene, deactivating object");
            this.gameObject.SetActive(false);
        }
    }

    private void Start() {
        _spawnDelay = concurrentSpawnDelay;
    }

    private void Update() {
        if(Time.time < initialSpawnDelay) return;
        
        if (Time.time > _timeLastSpawn + _spawnDelay) {
            _spawnDelay = concurrentSpawnDelay * Mathf.Pow(1 - decayFactor, Time.time);
            //spawn new instance
        }
    }

    private void ProcessSpawnEntity(GameObject entity) {
        if (_spawnComponents.Count == 0) return;
        
        var randomIndex = 0;
        
        if (_spawnComponents.Count > 1) {
            do randomIndex = Random.Range(0, _spawnComponents.Count -1);
            while (randomIndex == _lastSpawnAtIndex);
        }
        
        _spawnComponents[randomIndex].SpawnInstance(entity);
        _lastSpawnAtIndex = randomIndex;
        _timeLastSpawn = Time.time;
    }
}