using System;
using UnityEngine;

namespace BreadJ.MiniJam214
{
    [Serializable]
    public struct FloatRange
    {
        [Min(0f)] public float Min;
        [Min(0f)] public float Max;

        public float Width => Max - Min;
    }
}
