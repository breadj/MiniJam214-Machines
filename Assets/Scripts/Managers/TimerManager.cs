using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace BreadJ.MiniJam214
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance;

        [SerializeField] private TimerConfig config;

        [SerializeField] private Image timerBar;
        [SerializeField] private TextMeshProUGUI timeLeftText;
        [SerializeField] private TextMeshProUGUI totalTimeText;

        private bool isTimerActive;
        private float timeLeft;
        private float totalTime;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            timeLeft = config.MaxTime;      // starts with max time left
            totalTime = 0f;
        }

        private void Start()
        {
            isTimerActive = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (isTimerActive)
            {
                timeLeft -= Time.deltaTime;
                totalTime += Time.deltaTime;
                if (timeLeft <= 0f)
                {
                    // +timeLeft so Time.deltaTime doesn't overshoot the actual amount if the time since the last frame makes timeLeft < 0
                    totalTime += timeLeft;
                    timeLeft = 0f;
                    isTimerActive = false;

                    LoseGame();
                }

                SetBarFill();
                SetUIText();
            }
        }

        private void LoseGame()
        {
            // do stuff
        }

        #region UI
        private void SetBarFill()
        {
            float percentage = timeLeft / config.MaxTime;
            timerBar.fillAmount = Mathf.Clamp01(percentage);
        }

        private void SetUIText()
        {
            float truncTimeLeft = math.trunc(timeLeft * 10f) / 10f;
            timeLeftText.SetText($"{truncTimeLeft:0.0}s");

            float truncTotalTime = math.trunc(totalTime * 10f) / 10f;
            totalTimeText.SetText($"{truncTotalTime:0.0}s");
        }
        #endregion UI

        public void GainTime(float amount)
        {
            timeLeft = (timeLeft + amount) % config.MaxTime;
        }

        public void LoseTime(float amount)
        {
            timeLeft = (timeLeft - amount) % config.MaxTime;
            if (timeLeft <= 0f)
            {
                // +timeLeft so Time.deltaTime doesn't overshoot the actual amount if the time since the last frame makes timeLeft < 0
                totalTime += timeLeft;
                timeLeft = 0f;
                isTimerActive = false;

                LoseGame();
            }
        }
    }
}
