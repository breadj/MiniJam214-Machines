using System.Collections.Generic;
using UnityEngine;

namespace BreadJ.MiniJam214
{
    [RequireComponent(typeof(Collider2D))]
    public class SortedZone : MonoBehaviour
    {
        [SerializeField] private RobotColour Colour;

        private Collider2D trigger;
        private ContactFilter2D triggerFilter;
        private bool robotsCollectedThisFrame = false;

        private void Awake()
        {
            trigger = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            triggerFilter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = ToolManager.Instance.RobotsLayerGroup.Mask
            };

            robotsCollectedThisFrame = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Trigger entered by " + collision.gameObject.name);
            if (robotsCollectedThisFrame)
                return;

            CountSortedRobots();
        }

        private void CountSortedRobots()
        {
            int blueCount = 0;
            int redCount = 0;

            List<Robot> robots = GetRobotsInTrigger();
            foreach (Robot robot in robots) 
            { 
                if (robot.Colour == RobotColour.Blue)
                {
                    blueCount++;
                }
                else if (robot.Colour == RobotColour.Red)
                {
                    redCount++;
                }

                if (robot.Colour == Colour)
                {
                    robot.Teleport();
                }
                else
                {
                    robot.Explode();
                }
            }

            //Debug.Log($"Blue: {blueCount}, Red: {redCount}");

            if (Colour == RobotColour.Blue)
            {
                GameplayManager.Instance.ReportRobotBatchSort(blueCount, redCount);
            }
            else if (Colour == RobotColour.Red)
            {
                GameplayManager.Instance.ReportRobotBatchSort(redCount, blueCount);
            }
        }

        private List<Robot> GetRobotsInTrigger()
        {
            List<Collider2D> colliders = new();
            trigger.Overlap(triggerFilter, colliders);

            List<Robot> robots = new();
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Robot robot)) {
                    robots.Add(robot);
                }
            }

            return robots;
        }
    }
}
