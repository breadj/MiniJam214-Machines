using UnityEngine;

namespace BreadJ.MiniJam214
{
    public class DestroyAfterAnimation : MonoBehaviour
    {
        public System.Action OnAnimationEnd;

        public void DestroySelf()
        {
            OnAnimationEnd?.Invoke();
            Destroy(gameObject);
        }
    }
}
