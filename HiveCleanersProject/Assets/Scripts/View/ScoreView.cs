using System.Collections;
using System.Globalization;
using Components.Timer;
using TMPro;
using UnityEngine;

namespace View {
    public class ScoreView : MonoBehaviour {
        [SerializeField] private ScoreManager manager;

        [Header("text elements")]
        [SerializeField] private TextMeshProUGUI scoreText = null;
        [SerializeField] private TextMeshProUGUI comboText = null;

        private void Awake() {
            comboText.enabled = false;
        }

        private void OnEnable() {
            manager.comboTimer.OnTimerStart += ShowComboText;
            manager.comboTimer.OnTimerEnd += HideComboText;
            manager.OnScoreChanged += UpdateScore;
        }

        private void OnDisable() {
            manager.comboTimer.OnTimerStart -= ShowComboText;
            manager.comboTimer.OnTimerEnd -= HideComboText;
            manager.OnScoreChanged -= UpdateScore;
        }

        private void LateUpdate() => comboText.color = new Color(1,1,0, manager.comboTimer.CurrentTime / manager.comboTimer.InitialTime);
        
        private void ShowComboText() => comboText.enabled = true;
        private void HideComboText() => comboText.enabled = false;

        public void UpdateScore() {
            scoreText.text = manager.Score.ToString(CultureInfo.InvariantCulture);
            comboText.text = $"+{manager.Combo}";
        }
    }
}
