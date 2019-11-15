using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util.InteractionSystem {
    [Serializable]
    public class InteractiveObject : MonoBehaviour, IInteractive {
        [SerializeField] private bool destroyOnInteraction;
        [SerializeField] private bool interactive = true;

        public UnityEvent onInteraction;

        public Vector3 Position => transform.position;
        public bool Destroyed { get; private set; }

        public virtual void OnInteract() {
            if (interactive) onInteraction.Invoke();
            if (destroyOnInteraction) Destroy(gameObject);
        }

        public event Action<InteractiveObject> OnObjectDestroyed;

        private void OnDestroy() {
            DisposeObject();
        }

        private void DisposeObject() {
            if (!Destroyed) {
                Destroyed = true;
                OnObjectDestroyed?.Invoke(this);
            }
        }
    }
}