using System.Collections.Generic;
using UnityEngine;

namespace Systems.Events {
    [CreateAssetMenu(fileName = "newSignal", menuName = "signals")]
    public class Signal : ScriptableObject {
        private readonly List<SignalListener> _listeners = new List<SignalListener>();

        public void Raise() {
            for (var i = 0; i < _listeners.Count; i++) _listeners[i].OnEventRaised();
        }

        public void Subscribe(SignalListener signalListener) => _listeners.Add(signalListener);
        public void Unsubscribe(SignalListener signalListener) => _listeners.Remove(signalListener);
    }
}