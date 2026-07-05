using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BreadJ.MiniJam214
{
    public class Magnet : Tool
    {
        [SerializeField] private float radius = 3f;

        private readonly List<Robot> heldRobots = new();

#if UNITY_EDITOR
        public override void OnEquip()
        {
            Debug.Log("Equipped Magnet");
        }

        public override void OnUnqeuip()
        {
            Debug.Log("Unequipped Magnet");
        }
#endif

        public override void StartUse(Vector2 worldPos)
        {
            PickUpRobot(worldPos);
        }

        public override void UpdateUse(Vector2 worldPos)
        {
            transform.position = worldPos;
        }

        public override void EndUse(Vector2 worldPos)
        {
            DropRobot();
        }

        private void PickUpRobot(Vector2 worldPos)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(worldPos, radius, ToolManager.Instance.RobotsLayerGroup.Mask);
            foreach (Collider2D hit in hits)
            {
                if (!hit.TryGetComponent(out Robot robot))
                    continue;

                robot.PickUp(transform);
                heldRobots.Add(robot);
            }
        }

        private void DropRobot()
        {
            foreach (Robot robot in heldRobots)
            {
                robot.PutDown();
            }

            heldRobots.Clear();
        }
    }
}
