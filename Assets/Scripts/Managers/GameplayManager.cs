using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace BreadJ.MiniJam214
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance;

        [SerializeField] private GameObject winScreen;
        [SerializeField] private TextMeshProUGUI finalTimeText;

        [SerializeField] private List<TimeLossGain> difficultyTimeValues;
        private int currentDifficultyIndex = 0;
        private bool maxDifficultyReached = false;
        private float currentRobotDeathTimeLoss => difficultyTimeValues[currentDifficultyIndex].Loss;
        private float currentRobotSortTimeGain => difficultyTimeValues[currentDifficultyIndex].Gain;

        private float gameTime = 0f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            gameTime += Time.deltaTime;
            if (!maxDifficultyReached && gameTime >= difficultyTimeValues[currentDifficultyIndex + 1].Time)
            {
                currentDifficultyIndex++;
                if (currentDifficultyIndex + 1 == difficultyTimeValues.Count)
                {
                    maxDifficultyReached = true;
                }
            }
        }

        public void ReportRobotDeath(Robot robot)
        {
            TimerManager.Instance.LoseTime(currentRobotDeathTimeLoss);
        }

        public void ReportRobotSorted(Robot robot)
        {
            TimerManager.Instance.GainTime(currentRobotSortTimeGain);
        }

        public void ReportRobotBatchSort(int correctCount, int incorrectCount)
        {
            float timeGain = correctCount * currentRobotSortTimeGain;
            float timeLoss = incorrectCount * currentRobotDeathTimeLoss;

            float totalGain = timeGain - timeLoss;
            if (totalGain > 0)
            {
                TimerManager.Instance.GainTime(totalGain);
            }
            else if (totalGain < 0)
            {
                TimerManager.Instance.LoseTime(-totalGain);
            }
        }

        public void LoseGame(float finalTime)
        {
            SpawnManager.Instance.SpawningAllowed = false;
            winScreen.SetActive(true);
            float truncFinalTime = math.trunc(finalTime * 10f) / 10f;
            finalTimeText.SetText($"{truncFinalTime:0.0}s");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (difficultyTimeValues.Count < 2)
                Debug.LogWarning("GameplayManager.DifficultyTimeValues requires at least two values");

            // sorts based on TimeLossGain.Time
            difficultyTimeValues.Sort(Comparer<TimeLossGain>.Create((a, b) => (int)Mathf.Sign(a.Time - b.Time)));
        }
#endif
    }
}
