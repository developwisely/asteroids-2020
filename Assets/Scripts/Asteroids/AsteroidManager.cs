using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Asteroids
{
    public enum AsteroidTypes { Small, Medium, Large };

    public class AsteroidManager : MonoBehaviour
    {
        [SerializeField]
        public GameObject[] asteroids;

        // Level manager
        // private LevelManager _levelManager;

        // The number of asteroids currently on screen
        private int _currentNumAsteroids;

        // The total pool point value of the asteroids on screen
        private int _currentAsteroidPoints;

        // The lowest point value in the asteroid pool
        private int _lowestAsteroidValue;

        // The rate at which asteroids spawn (in seconds)
        public float spawnRate = 3;
        private float _canSpawnIn;

        private void Start()
        {
            //_levelManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
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
            if (GetTotalAvailableSpawnPoints() > _lowestAsteroidValue)
            {
                SpawnRandomAsteroid();
            }
        }

        private int GetTotalAvailableSpawnPoints()
        {
            //return _levelManager.GetMaxAsteroidPoints() - _currentAsteroidPoints;
            return 200 - _currentAsteroidPoints;
        }

        public void SpawnRandomAsteroid()
        {
            // Check cooldown before spawning
            if (_canSpawnIn > 0) return;

            // Max points for the level
            // _levelManager.GetMaxAsteroidPoints()
            // var availablePoints = _levelManager.GetMaxAsteroidPoints() - _currentAsteroidPoints
            // TODO: Implement level manager points
            int availablePoints = 200;

            // Create a pool of asteroids to choose from if the point value is greater than available points
            var randomPool = GetRandomPoolToSpawn(availablePoints);

            // Select a random asteroid to spawn
            int randomIndex = Random.Range(0, randomPool.Count);

            // Select random spawn point (off screen)
            // TODO: use outer bounds of map
            Vector3 randomPosition = new Vector3(50, 0, 50);

            // Spawn the Asteroid
            SpawnAsteroid(randomPool[randomIndex], randomPosition);

            // Reset the cooldown
            _canSpawnIn = spawnRate;
        }

        public GameObject SpawnAsteroid(GameObject asteroid, Vector3 position)
        {
            // Spawn the new asteroid
            GameObject newAsteroid = Instantiate(asteroid, position, Quaternion.identity) as GameObject;

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

            // Handle breaking into new asteroids
            if (type == AsteroidTypes.Medium || type == AsteroidTypes.Large)
            {
                BreakAsteroid(type, position, pointValue);
            }
        }

        private void BreakAsteroid(AsteroidTypes oldType, Vector3 oldPosition, int oldPointValue)
        {
            oldPosition.y = 0;

            // Subtract 1 to prevent spawning the same asteroid type
            int breakPoolPoints = oldPointValue;

            // 75% spawn chance first
            //bool spawn = Random.value > 0.25f;
            bool spawn = true;

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

                // Update pool points and spawn chance to 50%
                breakPoolPoints -= randomPool[randomIndex].GetComponent<Asteroid>().pointValue;
                //spawn = Random.value > 0.5f;
                spawn = true;
            }

            // Spawn the asteroids


            /*--------------------------------
            Vector3 boundExtents = newAsteroid.GetComponent<MeshCollider>().bounds.extents;
            float checkRadius = Mathf.Round(Mathf.Max(Mathf.Max(boundExtents.x, boundExtents.y), boundExtents.z) * 10.0f) / 10.0f;
            Debug.Log("Check Radius: " + checkRadius);
            bool canSpawnHere = !Physics.CheckSphere(position, checkRadius);
            Debug.Log("Can Spawn Here: " + canSpawnHere);
            int attempts = 0;
            /*
            while (!canSpawnHere)
            {
                position += new Vector3(Random.Range(-2, 2) * checkRadius * 2, 0, Random.Range(-2, 2) * checkRadius * 2);
                position.y = 0;
                canSpawnHere = !Physics.CheckSphere(position, checkRadius);
                Debug.Log("Position: " + position + " Can Spawn Here: " + canSpawnHere);
                attempts++;

                if (attempts > 29)
                {
                    Debug.Log("Too many attempts!");
                    Destroy(newAsteroid);
                    break;
                }
            }

            if (canSpawnHere)
            {
                newAsteroid.transform.position = position;
            }
            -----------------------------------------*/
        }

        public void UpdateSpawnRate(float newRate)
        {
            spawnRate = newRate;
        }
    }
}