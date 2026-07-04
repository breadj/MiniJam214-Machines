using System.Collections.Generic;
using UnityEngine;

namespace BreadJ.MiniJam214
{
    public class ToolManager : MonoBehaviour
    {
        public static ToolManager Instance { get; private set; }

        [SerializeField] private LayerGroup robotsLayerGroup;
        public LayerGroup RobotsLayerGroup => robotsLayerGroup;

        private bool usingTool = false;

        [SerializeField] private List<Tool> tools;
        private int toolIndex = 0;

        private Vector2 mousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (usingTool)
            {
                tools[toolIndex].UpdateUse(mousePos);
            }
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
            tools[toolIndex].OnUnqeuip();

            toolIndex = (toolIndex + 1) % tools.Count;
            
            tools[toolIndex].OnEquip();
        }

        public void PrevTool()
        {
            tools[toolIndex].OnUnqeuip();

            toolIndex = (toolIndex - 1) % tools.Count;

            tools[toolIndex].OnEquip();
        }

        public void BeginUsingTool()
        {
            usingTool = true;
            tools[toolIndex].StartUse(mousePos);
        }

        public void StopUsingTool()
        {
            usingTool = false;
            tools[toolIndex].EndUse(mousePos);
        }
    }
}
