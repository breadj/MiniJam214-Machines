using UnityEngine;
using UnityEngine.InputSystem;

namespace BreadJ.MiniJam214
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private ToolManager toolManager;

        public void UseTool(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                toolManager.BeginUsingTool();
            }
            else if (ctx.canceled)
            {
                toolManager.StopUsingTool();
            }
        }

        public void ChangeTool(InputAction.CallbackContext ctx)
        {
            float scroll = ctx.ReadValue<Vector2>().y;

            if (scroll > 0)
            {
                toolManager.NextTool();
            }
            else if (scroll < 0)
            {
                toolManager.PrevTool();
            }
        }
    }
}
