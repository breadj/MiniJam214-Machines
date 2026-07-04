using UnityEngine;

namespace BreadJ.MiniJam214
{
    public class Tweezers : Tool
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public override void StartUse(Vector2 worldPos)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateUse(Vector2 worldPos)
        {
            transform.position = worldPos;
        }

        public override void EndUse(Vector2 worldPos)
        {
            throw new System.NotImplementedException();
        }

        private void PickUpRobot(Vector2 worldPos)
        {
            Collider2D hit = Physics2D.OverlapPoint(worldPos, ToolManager.Instance.RobotsLayerGroup.Mask);
            if (hit == null)
                return;

            
        }
    }
}
