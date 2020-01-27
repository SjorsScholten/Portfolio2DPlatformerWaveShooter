﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components {
    [RequireComponent(typeof(Collider2D))]
    public class TargetComponent : MonoBehaviour {
        public event Action<float> OnHit;

        public void ProcessTakeHit(float damage = 0) {
            Debug.Log($"{this.name}, has been hit");
            OnHit?.Invoke(damage);
        } 
    }
}