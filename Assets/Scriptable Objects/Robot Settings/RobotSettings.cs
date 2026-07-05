using UnityEngine;

namespace BreadJ.MiniJam214
{
    [CreateAssetMenu(fileName = "RobotSettings", menuName = "Scriptable Objects/RobotSettings")]
    public class RobotSettings : ScriptableObject
    {
        [Header("Wander")]
        [SerializeField] private float moveSpeed = 5f;

        [Tooltip("Time (in seconds) between the min and max amount of time a robot waits until moving in a random direction")]
        [SerializeField] private FloatRange waitTime;
        [Tooltip("Time (in seconds) between the min and max amount of time a robot spends moving in a random direction")]
        [SerializeField] private FloatRange walkDuration;

        public float MoveSpeed => moveSpeed;
        public FloatRange WanderWaitTime => waitTime;
        public FloatRange WanderWalkDuration => walkDuration;
    }
}
