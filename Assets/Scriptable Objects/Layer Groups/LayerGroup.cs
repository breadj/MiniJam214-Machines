using UnityEngine;

namespace BreadJ.MiniJam214
{
    [CreateAssetMenu(fileName = "LayerGroup", menuName = "Scriptable Objects/LayerGroup")]
    public class LayerGroup : ScriptableObject
    {
        [SerializeField] private LayerMask layers;
        public LayerMask Mask => layers;
    }
}
