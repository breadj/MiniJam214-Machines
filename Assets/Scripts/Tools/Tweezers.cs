using TMPro;
using UnityEngine;

namespace BreadJ.MiniJam214
{
    public class Tweezers : Tool
    {
        [SerializeField] private TextMeshProUGUI tweezersText;

        private Robot heldRobot = null;

        public override void OnEquip(Transform pointer)
        {
            transform.SetParent(pointer);
            transform.localPosition = Vector3.zero;

            tweezersText.fontStyle |= FontStyles.Bold;
            tweezersText.text = "<u>Tweezers</u>";
        }

        public override void OnUnqeuip()
        {
            transform.SetParent(null);

            if (heldRobot != null)
            {
                DropRobot();
            }

            tweezersText.fontStyle &= ~FontStyles.Bold;
            tweezersText.text = "Tweezers";
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
