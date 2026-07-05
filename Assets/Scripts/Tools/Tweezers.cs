using UnityEngine;

namespace BreadJ.MiniJam214
{
    public class Tweezers : Tool
    {
        private Robot heldRobot = null;

        public override void OnEquip()
        {
            Debug.Log("Equipped Tweezers");
        }

        public override void OnUnqeuip()
        {
            Debug.Log("Unequipped Tweezers");

            if (heldRobot != null)
            {
                DropRobot();
            }
        }

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
            Collider2D hit = Physics2D.OverlapPoint(worldPos, ToolManager.Instance.RobotsLayerGroup.Mask);
            if (hit == null)
                return;

            if (!hit.TryGetComponent(out Robot robot))
                return;

            robot.PickUp(transform);
            heldRobot = robot;
        }

        private void DropRobot()
        {
            if (heldRobot == null)
                return;

            heldRobot.PutDown();
            heldRobot = null;
        }
    }
}
