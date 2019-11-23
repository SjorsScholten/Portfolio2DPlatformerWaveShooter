using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components {
    [RequireComponent(typeof(Collider2D))]
    public class TargetComponent : MonoBehaviour {

        public OnHitEvent OnHit = new OnHitEvent();

        public void ProcessTakeHit(float damage = 0) {
            Debug.Log($"{this.name}, has been hit");
            OnHit?.Invoke(damage);
        } 
    }

    [Serializable]
    public class OnHitEvent : UnityEvent<float> { }
}