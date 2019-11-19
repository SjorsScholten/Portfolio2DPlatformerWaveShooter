﻿using UnityEngine;
using UnityEngine.Events;

namespace Components {
    [RequireComponent(typeof(Collider2D))]
    public class TargetComponent : MonoBehaviour {
        public void ProcessTakeHit() => Debug.Log($"you hit: {this.name}");
    }
}