using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BreadJ.MiniJam214
{
    public class ToolManager : MonoBehaviour
    {
        public static ToolManager Instance { get; private set; }

        [SerializeField] private LayerGroup robotsLayerGroup;
        public LayerGroup RobotsLayerGroup => robotsLayerGroup;

        [SerializeField] private List<Tool> tools;
        private int toolIndex = 0;
        private Tool currentTool => tools[toolIndex];

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            currentTool.OnEquip(InputManager.Instance.PointerTarget);
        }

        private void Update()
        {
            
        }

        private void OnValidate()
        {
            if (tools.Count < 1)
            {
                Debug.LogError("ToolManager requires at least one tool in the Tools list");
            }
        }

        public void NextTool()
        {
            currentTool.OnUnqeuip();

            toolIndex = WrapToolIndex(toolIndex + 1);
            
            currentTool.OnEquip(InputManager.Instance.PointerTarget);
        }

        public void PrevTool()
        {
            currentTool.OnUnqeuip();

            toolIndex = WrapToolIndex(toolIndex - 1);

            currentTool.OnEquip(InputManager.Instance.PointerTarget);
        }

        private int WrapToolIndex(int newIndex)
        {
            return ((newIndex % tools.Count) + tools.Count) % tools.Count;
        }

        public void BeginUsingTool()
        {
            currentTool.StartUse(currentTool.transform.position);
        }

        public void UpdateToolPosition(Vector2 worldPos)
        {
            currentTool.UpdateUse(worldPos);
        }

        public void StopUsingTool()
        {
            currentTool.EndUse(currentTool.transform.position);
        }
    }
}
