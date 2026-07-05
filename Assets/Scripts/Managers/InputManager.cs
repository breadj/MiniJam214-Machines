using UnityEngine;
using UnityEngine.InputSystem;

namespace BreadJ.MiniJam214
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        private Transform pointer;
        public Transform PointerTarget => pointer;

        private bool scrolledThisFrame = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            pointer = new GameObject("Pointer").transform;
        }

        private void Update()
        {
            scrolledThisFrame = false;
        }

        public void UseTool(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                ToolManager.Instance.BeginUsingTool();
            }
            else if (ctx.canceled)
            {
                ToolManager.Instance.StopUsingTool();
            }
        }

        public void ChangeTool(InputAction.CallbackContext ctx)
        {
            if (scrolledThisFrame)
                return;
            scrolledThisFrame = true;

            float scroll = ctx.ReadValue<Vector2>().y;
            if (scroll > 0)
            {
                ToolManager.Instance.NextTool();
            }
            else if (scroll < 0)
            {
                ToolManager.Instance.PrevTool();
            }
        }

        public void MoveTool(InputAction.CallbackContext ctx)
        {
            Vector2 ssMousePos = ctx.ReadValue<Vector2>();
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(ssMousePos);

            pointer.position = worldMousePos;
        }
    }
}
