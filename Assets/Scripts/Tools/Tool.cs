using UnityEngine;

namespace BreadJ.MiniJam214
{
    public abstract class Tool : MonoBehaviour
    {
        public virtual void OnEquip() { }
        public virtual void OnUnqeuip() { }
        public abstract void StartUse(Vector2 worldPos);
        public abstract void UpdateUse(Vector2 worldPos);
        public abstract void EndUse(Vector2 worldPos);
    }
}
