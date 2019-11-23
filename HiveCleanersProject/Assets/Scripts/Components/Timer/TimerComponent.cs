using UnityEngine;
using UnityEngine.Events;

namespace Components {
    public class TimerComponent : MonoBehaviour {
        [SerializeField] private Timer.Timer timer = new Timer.Timer();

        [SerializeField] private bool startAtBeginning = false;
        
        public UnityEvent onTick = new UnityEvent();
        public UnityEvent onTimerStart = new UnityEvent();
        public UnityEvent onTimerEnd = new UnityEvent();

        private void Start() {
            if(startAtBeginning) StartTimer();
        }

        private void OnEnable() {
            timer.OnTimerTick += ProcessTick;
            timer.OnTimerStart += ProcessStart;
            timer.OnTimerEnd += ProcessEnd;
        }

        private void OnDisable() {
            timer.OnTimerTick -= ProcessTick;
            timer.OnTimerStart -= ProcessStart;
            timer.OnTimerEnd -= ProcessEnd;
        }

        private void Update() => timer.Update(Time.deltaTime);

        public void StartTimer() => timer.Start();

        public void PauseTimer() => timer.Pause();

        public void ResetTimer() => timer.Reset();

        public void StopTimer() => timer.Stop();

        private void ProcessStart() => onTimerStart?.Invoke();

        private void ProcessTick() => onTick?.Invoke();

        private void ProcessEnd() => onTimerEnd?.Invoke();

        public float SecondsTime => timer.CurrentTime;
    }
}