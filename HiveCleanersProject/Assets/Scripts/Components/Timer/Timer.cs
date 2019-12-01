using System;
using UnityEngine;

namespace Components.Timer {
    [Serializable]
    public class Timer {
        
        [SerializeField] private float initialTime = 0f;
        [SerializeField] private float timeScale = 1f;
        [SerializeField] private bool countDown = false;
        [SerializeField] private bool enableTick = false;
        [SerializeField] private float tickTime = 0f;

        private float _currentTime = 0f;
        private float _timeLastTick = 0f;

        private bool _isAwake = false;

        public event Action OnTimerStart;
        public event Action OnTimerTick;
        public event Action OnTimerEnd;

        
        public Timer(float initialTime = 0f, bool countDown = false) {
            this.initialTime = initialTime;
            _currentTime = this.initialTime;
            this.countDown = countDown;
        }
        
        public void Update(float deltaTime) {
            if(!_isAwake) return;
            
            _currentTime += deltaTime * TimeScale * (countDown ? -1 : 1);

            CheckTimerTick();
            CheckTimerEnd();
        }

        
        private void CheckTimerTick() {
            if (!enableTick || !(CurrentTime > _timeLastTick + tickTime)) return;
            OnTimerTick?.Invoke();
            _timeLastTick = CurrentTime;
        }

        private void CheckTimerEnd() {
            if(CurrentTime > 0) return;
            Stop();
        }

        
        public void Start() {
            if(_isAwake) return;
            OnTimerStart?.Invoke();
            _isAwake = true;
        }

        public void Pause() {
            if(!_isAwake) return;
            _isAwake = false;
        }

        public void Stop() {
            if(!_isAwake) return;
            Pause();
            Reset();
            OnTimerEnd?.Invoke();
        }

        public void Reset() => _currentTime = initialTime;

        public void AddTime(float amount) => _currentTime += amount;

        
        public float CurrentTime => _currentTime;
        
        public float TimeScale {
            get => timeScale;
            set => timeScale = value;
        }

        public bool IsAwake => _isAwake;

        public float InitialTime => initialTime;
    }
}