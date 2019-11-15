using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util.InteractionSystem {
    /**
     *
     * TODO: find a way to get the section of a circle
     * TODO: player cant interact with objects that are obscured
     * 
     * if there are no objects in range:
     * - if there is a selected object: set current interactive to null
     * - else: do nothing
     *
     * if there is one object in range:
     * - check if object is in section
     *     + object is in section -> set object as current interact-able
     *     + object isn't in section -> do nothing
     *
     * if there is more than one object in range:
     * - filter out the objects in sector
     *     + there are no objects in section -> do nothing
     *     + there is more than one object in section -> Sort array by shortest distance to player
     * - set first object as current interact-able
     */

    [Serializable]
    public class InteractiveObjectManager : MonoBehaviour {
        private readonly List<InteractiveObject> objectsInRange = new List<InteractiveObject>();

        private Collider _collider;
        private InteractiveObject[] _inSection;
        public InteractiveObject CurrentInteractive { get; private set; }

        private void Awake() {
            _collider = GetComponent<Collider>();
        }

        private void Update() {
            if (objectsInRange.Count > 0) UpdateInteractive();
        }

        private void OnTriggerEnter(Collider other) {
            var interactiveObj = other.GetComponent<InteractiveObject>();
            if (interactiveObj) AddToList(interactiveObj);
        }

        private void OnTriggerExit(Collider other) {
            var interactiveObj = other.GetComponent<InteractiveObject>();
            if (interactiveObj) RemoveFromList(interactiveObj);
        }

        private void AddToList(InteractiveObject obj) {
            obj.OnObjectDestroyed += RemoveFromList;
            objectsInRange.Add(obj);
        }

        private void RemoveFromList(InteractiveObject obj) {
            objectsInRange.Remove(obj);
            obj.OnObjectDestroyed -= RemoveFromList;
            if (objectsInRange.Count <= 0) CurrentInteractive = null;
        }

        private void UpdateInteractive() {
            _inSection = objectsInRange.Where(obj => CheckIfPointInSection(obj.Position)).ToArray();

            if (_inSection.Length > 1)
                Array.Sort(_inSection,
                    (a, b) => {
                        var deltaA = Vector3.Distance(a.Position, _collider.bounds.center);
                        var deltaB = Vector3.Distance(b.Position, _collider.bounds.center);
                        return deltaA.CompareTo(deltaB);
                    }
                );

            foreach (var o in _inSection) {
                if (IsObjectObscured(o)) continue;
                CurrentInteractive = o;
                break;
            }
        }

        private bool CheckIfPointInSection(Vector3 point) {
            return true;
        }

        private bool IsObjectObscured(InteractiveObject interactive) {
            /*
            var position = transform.position;
            return Physics.Linecast(position, interactive.Position) && Physics.Linecast(position + Vector3.up * _collider.bounds.max.y, interactive.Position);
            */

            return false;
        }
    }
}