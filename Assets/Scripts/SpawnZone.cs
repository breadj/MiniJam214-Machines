using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BreadJ.MiniJam214
{
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float staticDifficultyRating;
        [SerializeField] private List<Transform> spawnLocations;
        public float StaticDifficultyRating => staticDifficultyRating;
        public int NumberOfSpawnLocations => spawnLocations.Count;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Spawn(int howManyToSpawn)
        {
            if (howManyToSpawn > NumberOfSpawnLocations)
            {
                Debug.LogWarning($"{gameObject.name} only has {NumberOfSpawnLocations} spawn locations, but attempted to spawn {howManyToSpawn}");
                return;
            }

            List<int> spawnIndices;
            if (howManyToSpawn < NumberOfSpawnLocations)
            {
                spawnIndices = PickRandomDistinctSpawnIndices(howManyToSpawn);
            }
            else            // just uses all indices if possible
            {
                spawnIndices = Enumerable.Range(0, NumberOfSpawnLocations).ToList();
            }

            foreach (int spawnIndex in spawnIndices)
            {
                RobotColour robotColour = Random.value < 0.5f ? RobotColour.Blue : RobotColour.Red;
                Robot robot = Instantiate(SpawnManager.Instance.GetRobotPrefab(robotColour), 
                    spawnLocations[spawnIndex].position, Quaternion.identity)
                    .GetComponent<Robot>();
                robot.SpawnIn();
            }
        }

        private List<int> PickRandomDistinctSpawnIndices(int numToPick)
        {
            List<int> indices = Enumerable.Range(0, NumberOfSpawnLocations).ToList();
            Shuffle(indices);

            return indices.Take(numToPick).ToList();
        }

        private void Shuffle(List<int> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
