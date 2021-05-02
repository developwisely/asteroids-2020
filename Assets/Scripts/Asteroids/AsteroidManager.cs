using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public enum AsteroidTypes { Small, Medium, Large };

    public class AsteroidManager : MonoBehaviour
    {
        [SerializeField]
        public GameObject[] asteroids;

        // Level manager
        private LevelManager _levelManager;

        // The number of asteroids currently on screen
        private int _currentNumAsteroids;

        // The total pool point value of the asteroids on screen
        private int _currentAsteroidPoints;

        // The lowest point value in the asteroid pool
        private int _lowestAsteroidValue;

        // The rate at which asteroids spawn (in seconds)
        public float spawnRate = 3;
        private float _canSpawnIn;
        private float _spawnOffsetFromBounds = 20; // distance from bounds of camera to spawn

        // Camera bounds
        private ScreenBounds _screenBounds;

        private void Start()
        {
            _levelManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();

            _screenBounds = Camera.main.GetComponentInChildren<ScreenBounds>();

            _currentNumAsteroids = 0;
            _currentAsteroidPoints = 0;

            // Save the lowest asteroid point value
            int[] asteroidPointValues = new int[asteroids.Length];
            for (var a = 0; a < asteroids.Length; a++)
            {
                asteroidPointValues[a] = asteroids[a].GetComponent<Asteroid>().pointValue;
            }
            _lowestAsteroidValue = Mathf.Min(asteroidPointValues);

            // Set the cooldown for spawning
            _canSpawnIn = spawnRate;
        }

        private void Update()
        {
            // Update the cooldown
            _canSpawnIn -= Time.deltaTime;

            // Check if it's possible to spawn an asteroid via points
            if (GetTotalAvailableSpawnPoints() >= _lowestAsteroidValue)
            {
                SpawnRandomAsteroid();
            }
        }

        private int GetTotalAvailableSpawnPoints()
        {
            return _levelManager.GetMaxAsteroidPoints() - _currentAsteroidPoints;
        }

        public void SpawnRandomAsteroid()
        {
            // Check cooldown before spawning
            if (_canSpawnIn > 0) return;

            // Max points for the level
            int availablePoints = GetTotalAvailableSpawnPoints();

            // Create a pool of asteroids to choose from if the point value is greater than available points
            var randomPool = GetRandomPoolToSpawn(availablePoints);

            // Select a random asteroid to spawn
            int randomIndex = Random.Range(0, randomPool.Count);

            // Select random spawn point (off screen)
            Bounds screenBounds = _screenBounds.BOUNDS(); // Camera uses x,y
            Vector3 randomScreenLocation = _screenBounds.RANDOM_ON_SCREEN_LOCATION(); // Camera uses x,y

            float x = 0;
            float z = 0;
            switch(Random.Range(0, 4))
            {
                // Top
                case 0:
                    x = randomScreenLocation.x;
                    z = screenBounds.min.z - _spawnOffsetFromBounds;
                    break;

                // Right
                case 1:
                    x = screenBounds.max.x + _spawnOffsetFromBounds;
                    z = randomScreenLocation.z;
                    break;

                // Bottom
                case 2:
                    x = randomScreenLocation.x;
                    z = screenBounds.max.z + _spawnOffsetFromBounds;
                    break;

                // Left
                case 3:
                    x = screenBounds.min.x - _spawnOffsetFromBounds;
                    z = randomScreenLocation.z;
                    break;
            }

            Vector3 randomPosition = new Vector3(x, 0, z);

            // Spawn the Asteroid
            SpawnAsteroid(randomPool[randomIndex], randomPosition);

            // Reset the cooldown
            _canSpawnIn = spawnRate;
        }

        public GameObject SpawnAsteroid(GameObject asteroid, Vector3 position, float angle = 0f)
        {
            // Set default rotation
            Quaternion rotation = Quaternion.identity;
            if (angle > 0)
            {
                rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            }

            // Spawn the new asteroid
            GameObject newAsteroid = Instantiate(asteroid, position, rotation) as GameObject;

            // Update counters
            _currentAsteroidPoints += newAsteroid.GetComponent<Asteroid>().pointValue;
            _currentNumAsteroids++;

            // Return a reference
            return newAsteroid;
        }

        private List<GameObject> GetRandomPoolToSpawn(int poolPoints)
        {
            var randomPool = new List<GameObject>();
            for (int a = 0; a < asteroids.Length; a++)
            {
                if (asteroids[a].GetComponent<Asteroid>().pointValue <= poolPoints)
                {
                    randomPool.Add(asteroids[a]);
                }
            }

            return randomPool;
        }

        public void HandleDestroyedAsteroid(AsteroidTypes type, Vector3 position, int pointValue)
        {
            // Update counters
            _currentAsteroidPoints -= pointValue;
            _currentNumAsteroids--;
            _levelManager.UpdatePlayerPoints(pointValue);

            // Handle breaking into new asteroids
            if (type == AsteroidTypes.Medium || type == AsteroidTypes.Large)
            {
                BreakAsteroid(type, position, pointValue);
            }
        }

        private void BreakAsteroid(AsteroidTypes oldType, Vector3 oldPosition, int oldPointValue)
        {
            oldPosition.y = 0;

            // Subtract 1 to prevent spawning the same asteroid type (??)
            int breakPoolPoints = oldPointValue;

            // 90% spawn chance first
            bool spawn = Random.value > 0.1f;

            // Get random pool & remove destroyed type
            var randomPool = GetRandomPoolToSpawn(breakPoolPoints);
            for (int a = 0; a < randomPool.Count; a++)
            {
                if (randomPool[a].GetComponent<Asteroid>().type == oldType)
                {
                    randomPool.RemoveAt(a);
                    break;
                }
            }

            // Loop until no more break points or spawn false
            var asteroidsToSpawn = new List<GameObject>();
            while (breakPoolPoints > _lowestAsteroidValue && spawn)
            {
                // Pull a random qualified asteroid
                int randomIndex = Random.Range(0, randomPool.Count);

                // Add asteroid to spawn pool
                asteroidsToSpawn.Add(randomPool[randomIndex]);

                // Update pool points and spawn chance to 75%
                breakPoolPoints -= randomPool[randomIndex].GetComponent<Asteroid>().pointValue;
                spawn = Random.value > 0.25f;
            }

            // Set angles of breaking asteroids
            var angle = Random.Range(0, 360);
            var asteroidAngles = new List<int>();
            switch (asteroidsToSpawn.Count) {
                case 1:
                    asteroidAngles.Add(angle);
                    break;

                case 2:
                    asteroidAngles.Add(angle);
                    asteroidAngles.Add(angle + 180);
                    break;

                case 3:
                    asteroidAngles.Add(angle);
                    asteroidAngles.Add(angle + 120);
                    asteroidAngles.Add(angle + 240);
                    break;

                case 4:
                    asteroidAngles.Add(angle);
                    asteroidAngles.Add(angle + 90);
                    asteroidAngles.Add(angle + 180);
                    asteroidAngles.Add(angle + 270);
                    break;
            }

            // Spawn the asteroids
            for (int a = 0; a < asteroidsToSpawn.Count; a++)
            {
                SpawnAsteroid(asteroidsToSpawn[a], oldPosition, asteroidAngles[a]);
            }
        }

        public void UpdateSpawnRate(float newRate)
        {
            spawnRate = newRate;
        }
    }
}