using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BreadJ.MiniJam214
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Magnet : Tool
    {
        private SpriteRenderer sr;

        [SerializeField] private TextMeshProUGUI magnetText;

        [SerializeField] private float radius = 3f;

        private readonly List<Robot> heldRobots = new();

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            transform.localScale = 2 * radius * Vector3.one;
        }

        public override void OnEquip(Transform pointer)
        {
            transform.SetParent(pointer);
            transform.localPosition = Vector3.zero;

            sr.enabled = true;

            magnetText.fontStyle |= FontStyles.Bold;
            magnetText.text = "<u>Magnet</u>";
        }

        public override void OnUnqeuip()
        {
            transform.SetParent(null);
            sr.enabled = false;

            if (heldRobots.Count > 0)
            {
                DropRobots();
            }

            magnetText.fontStyle &= ~FontStyles.Bold;
            magnetText.text = "Magnet";
        }

        public override void StartUse(Vector2 worldPos)
        {
            PickUpRobots(worldPos);
        }

        public override void UpdateUse(Vector2 worldPos)
        {
            transform.position = worldPos;
        }

        public override void EndUse(Vector2 worldPos)
        {
            DropRobots();
        }

        private void PickUpRobots(Vector2 worldPos)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(worldPos, radius, ToolManager.Instance.RobotsLayerGroup.Mask);
            foreach (Collider2D hit in hits)
            {
                if ((transform.position - hit.transform.position).sqrMagnitude > radius * radius)
                    continue;

                if (!hit.TryGetComponent(out Robot robot))
                    continue;

                robot.PickUp(transform);
                heldRobots.Add(robot);
            }
        }

        private void DropRobots()
        {
            foreach (Robot robot in heldRobots)
            {
                robot.PutDown();
            }

            heldRobots.Clear();
        }
    }
}
