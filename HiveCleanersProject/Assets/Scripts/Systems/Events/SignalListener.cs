using UnityEngine;
using UnityEngine.Events;

namespace Systems.Events {
    public class SignalListener : MonoBehaviour {
        public Signal signal;
        public UnityEvent response;
        
        private void OnEnable() => signal.Subscribe(this);
        private void OnDisable() => signal.Unsubscribe(this);
        public void OnEventRaised() => response?.Invoke();
    }
}