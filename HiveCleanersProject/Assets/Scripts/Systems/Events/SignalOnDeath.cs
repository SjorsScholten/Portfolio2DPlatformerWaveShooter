using System;
using Systems;
using Systems.Events;
using UnityEngine;

namespace Entity {
    public class SignalOnDeath : MonoBehaviour {
        [SerializeField] private Signal onDeathSignal;

        private void Awake() {
            if(!onDeathSignal) enabled = false;
        }

        private void OnDestroy() => onDeathSignal.Raise();
    }
}