using UnityEngine;

namespace BreadJ.MiniJam214
{
    [CreateAssetMenu(fileName = "TimerConfig", menuName = "Scriptable Objects/TimerConfig")]
    public class TimerConfig : ScriptableObject
    {
        [SerializeField, Min(0f)] private float maxTime = 15f;
    }
}
