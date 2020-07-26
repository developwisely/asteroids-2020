using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

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
            SpawnAsteroid();
        }
    }

    private int GetTotalAvailableSpawnPoints()
    {
        //return _levelManager.GetMaxAsteroidPoints() - _currentAsteroidPoints;
        return 200 - _currentAsteroidPoints;
    }

    public void SpawnAsteroid()
    {
        // Check cooldown before spawning
        if (_canSpawnIn > 0) return;

        // Max points for the level
        // _levelManager.GetMaxAsteroidPoints()
        // var availablePoints = _levelManager.GetMaxAsteroidPoints() - _currentAsteroidPoints
        // TODO: Implement level manager points
        int availablePoints = 200;

        // Create a pool of asteroids to choose from if the point value is greater than available points
        var randomPool = new List<GameObject>();
        for (var a = 0; a < asteroids.Length; a++)
        {
            if (asteroids[a].GetComponent<Asteroid>().pointValue <= availablePoints)
            {
                randomPool.Add(asteroids[a]);
            }
        }

        // Select a random asteroid to spawn
        int randomIndex = Random.Range(0, randomPool.Count);

        // Select random spawn point (off screen)
        // TODO: use outer bounds of map
        Vector3 randomPosition = new Vector3(50, 0, 50);

        // Spawn the Asteroid
        GameObject newAsteroid = Instantiate(randomPool[randomIndex], randomPosition, Quaternion.identity) as GameObject;

        // Update manager variables
        _currentAsteroidPoints += newAsteroid.GetComponent<Asteroid>().pointValue;
        _currentNumAsteroids++;

        // Reset the cooldown
        _canSpawnIn = spawnRate;
    }

    public void HandleDestroyedAsteroid(GameObject asteroid)
    {
        _currentAsteroidPoints -= asteroid.GetComponent<Asteroid>().pointValue;
        _currentNumAsteroids--;
    }

    public void UpdateSpawnRate(float newRate)
    {
        spawnRate = newRate;
    }
}
