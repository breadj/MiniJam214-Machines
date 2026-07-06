using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BreadJ.MiniJam214
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance { get; private set; }

        [SerializeField] private GameObject blueRobotPrefab;
        [SerializeField] private GameObject redRobotPrefab;

        [SerializeField] private List<Vector2> timestampNumberToSpawn;
        private int currentNumberToSpawnIndex = 0;
        private int numberToSpawn => (int)timestampNumberToSpawn[currentNumberToSpawnIndex].y;
        private bool maxSpawnNumberReached = false;

        [SerializeField] private List<Vector2> timestampTimeBetweenSpawning;
        private int currentTimeBetweenSpawningIndex = 0;
        private float timeBetweenSpawning => timestampTimeBetweenSpawning[currentTimeBetweenSpawningIndex].y;
        private bool maxSpawnSpeedReached = false;
        private float timeSinceSpawn = 0f;

        [SerializeField] private List<SpawnZone> spawnZones;

        private float gameTime = 0f;
        private bool activelySpawning = true;

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
            gameTime += Time.deltaTime;
            if (!maxSpawnNumberReached && gameTime >= timestampNumberToSpawn[currentNumberToSpawnIndex + 1].x)
            {
                currentNumberToSpawnIndex++;
                if (currentNumberToSpawnIndex + 1 == timestampNumberToSpawn.Count)
                {
                    maxSpawnNumberReached = true;
                }
            }

            if (!maxSpawnSpeedReached && gameTime >= timestampTimeBetweenSpawning[currentTimeBetweenSpawningIndex + 1].x)
            {
                currentTimeBetweenSpawningIndex++;
                if (currentTimeBetweenSpawningIndex + 1 == timestampTimeBetweenSpawning.Count)
                {
                    maxSpawnSpeedReached = true;
                }
            }

            if (activelySpawning)
            {
                timeSinceSpawn += Time.deltaTime;
                if (timeSinceSpawn >= timeBetweenSpawning)
                {
                    timeSinceSpawn = 0f;
                    RequestSpawn();
                }
            }
        }

        private void RequestSpawn()
        {
            Dictionary<SpawnZone, int> allocatedSpawns = AllocateSpawns(numberToSpawn);
            
            foreach (SpawnZone spawnZone in allocatedSpawns.Keys)
            {
                spawnZone.Spawn(allocatedSpawns[spawnZone]);
            }
        }

        private class SpawnZoneAndFreeSpawns
        {
            public SpawnZone SpawnZone;
            public int FreeSpawns;

            public SpawnZoneAndFreeSpawns(SpawnZone spawnZone, int freeSpawns)
            {
                SpawnZone = spawnZone;
                FreeSpawns = freeSpawns;
            }
        }

        private Dictionary<SpawnZone, int> AllocateSpawns(int numberToSpawn)
        {
            Dictionary<SpawnZone, int> allocatedSpawns = new();

            List<SpawnZoneAndFreeSpawns> availableSpawnZones = spawnZones.Select(zone => new SpawnZoneAndFreeSpawns(zone, zone.NumberOfSpawnLocations)).ToList();

            int remaining = numberToSpawn;
            while (remaining > 0 && availableSpawnZones.Count > 0)
            {
                SpawnZoneAndFreeSpawns chosenSZAFS = WeightedSpawnZonePick(availableSpawnZones);

                int maxToSpawn = Mathf.Min(chosenSZAFS.FreeSpawns, remaining, 2);
                int actualNumberToSpawn = Random.Range(1, maxToSpawn + 1);

                if (!allocatedSpawns.ContainsKey(chosenSZAFS.SpawnZone))
                    allocatedSpawns[chosenSZAFS.SpawnZone] = 0;

                allocatedSpawns[chosenSZAFS.SpawnZone] += actualNumberToSpawn;

                chosenSZAFS.FreeSpawns -= actualNumberToSpawn;
                remaining -= actualNumberToSpawn;

                if (chosenSZAFS.FreeSpawns == 0)
                    availableSpawnZones.Remove(chosenSZAFS);
            }

            return allocatedSpawns;
        }

        private SpawnZoneAndFreeSpawns WeightedSpawnZonePick(List<SpawnZoneAndFreeSpawns> spawnZones)
        {
            float totalWeight = 0f;

            foreach (SpawnZoneAndFreeSpawns szafs in spawnZones)
                totalWeight += 1f / szafs.SpawnZone.StaticDifficultyRating;

            float chosen = Random.Range(0f, totalWeight);

            foreach (SpawnZoneAndFreeSpawns szafs in spawnZones)
            {
                chosen -= 1f / szafs.SpawnZone.StaticDifficultyRating;
                if (chosen <= 0f)
                    return szafs;
            }

            return spawnZones[^1];
        }

        public GameObject GetRobotPrefab(RobotColour robotColour)
        {
            return robotColour switch
            {
                RobotColour.Blue => blueRobotPrefab,
                RobotColour.Red => redRobotPrefab,
                _ => null
            };
        }
    }
}
