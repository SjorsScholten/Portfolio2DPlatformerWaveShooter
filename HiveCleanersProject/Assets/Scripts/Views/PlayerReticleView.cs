using Components.Attack;
using Input;
using UnityEngine;

namespace View {
    public class PlayerReticleView : MonoBehaviour {
        [SerializeField] private AttackComponent target;

        private RectTransform _transform;
        private Camera _camera;

        private void Awake() {
            _transform = GetComponent<RectTransform>();
            _camera = Camera.main;
        }

        private void LateUpdate() {
            //_transform.position = _camera.WorldToScreenPoint(target.Direction + target.Position);
        }
    }
}