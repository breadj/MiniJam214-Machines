using UnityEngine;
using UnityEngine.InputSystem;

namespace BreadJ.MiniJam214
{
    public class InputManager : MonoBehaviour
    {
        public void PickUp(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                // pick up
            }
            else if (ctx.canceled)
            {
                // put down
            }
        }

        public void ChangeTool(InputAction.CallbackContext ctx)
        {
            float scroll = ctx.ReadValue<Vector2>().y;

            if (scroll > 0)
            {
                // next tool
            }
            else if (scroll < 0)
            {
                // prev tool
            }
        }
    }
}
