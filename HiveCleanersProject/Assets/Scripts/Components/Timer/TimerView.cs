using System.Globalization;
using Components;
using Components.Timer;
using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class TimerView : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TimerComponent timer;

        private void Update() {
            var currentTime = timer.SecondsTime;
            text.text = $"{(currentTime / 3600) % 24:0}:{(currentTime / 60) % 60:00}:{currentTime % 60:00}";
        }
    }
}