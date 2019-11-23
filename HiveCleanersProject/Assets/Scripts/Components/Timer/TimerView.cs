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
            text.text = timer.SecondsTime.ToString(CultureInfo.InvariantCulture);
        }
    }
}