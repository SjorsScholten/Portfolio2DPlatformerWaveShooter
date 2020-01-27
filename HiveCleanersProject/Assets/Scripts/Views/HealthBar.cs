using System.Collections;
using Components;
using Components.Timer;
using Entity;
using UnityEngine;
using UnityEngine.UI;

namespace View {
    [RequireComponent(typeof(CanvasGroup))]
    public class HealthBar : MonoBehaviour {
        [SerializeField] private LivingEntity target;
        [SerializeField] private Image foreground;
        [SerializeField] private Vector2 offset = new Vector2(0f, 2f);
        [SerializeField] private Timer displayTimer = new Timer(1, true);
        
        private Transform _transform;
        private Transform _targetTransform;
        private Camera _camera;
        private CanvasGroup _elements;

        private void Awake() {
            _camera = Camera.main;
            
            _transform = GetComponent<Transform>();
            _targetTransform = target.GetComponent<Transform>();
            
            _elements = GetComponent<CanvasGroup>();
            
            displayTimer.OnTimerEnd += Hide;
        }
        
        private void Update() => displayTimer.Update(Time.deltaTime);

        private void LateUpdate() {
            _transform.position = _camera.WorldToScreenPoint((Vector2) _targetTransform.position + (Vector2) offset);
            if(displayTimer.IsAwake) _elements.alpha = displayTimer.CurrentTime / displayTimer.InitialTime;
        }

        public void SetFillPercent(float percentValue) {
            foreground.fillAmount = percentValue;
            
            if(percentValue < 0.99f) Show();
            else {
                displayTimer.Reset();
                displayTimer.Start();
            }
        }

        private void Show() => _elements.alpha = 1;
        private void Hide() => _elements.alpha = 0;
    }
}
