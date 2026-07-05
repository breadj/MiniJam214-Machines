using UnityEngine;

namespace BreadJ.MiniJam214
{
    [System.Serializable]
    public struct TimeLossGain
    {
        [Min(0f)] public float Time;
        [Min(0f)] public float Loss;
        [Min(0f)] public float Gain;
    }
}
