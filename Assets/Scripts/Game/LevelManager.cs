using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Per level multiplier of pool points for level clear.")]
    public float poolMultiplier;
    [Header("Per level multiplier of max points on screen.")]
    public float maxMultiplier;

    // make private
    public int _currentLevel = 1;

    // Counters for the level
    private int _levelPlayerPoints;
    private int _levelPoolPoints;
    private int _levelMaxPointsOnScreen;

    // Default algorithm values
    private int _baseLevelPoolPoints = 2000;
    private int _baseLevelMaxPointsOnScreen = 200;

    private void Start()
    {
        SetLevelRequirements();
    }

    private void Update()
    {
        // what to do here? spawn powerups or something (some timer for powerups and random chance?)
    }

    private void SetLevelRequirements()
    {
        // Set to level 1 if not set
        if (_currentLevel < 0)
        {
            _currentLevel = 1;
        }

        _levelPoolPoints = Mathf.RoundToInt((_currentLevel * _baseLevelPoolPoints) * poolMultiplier);
        _levelMaxPointsOnScreen = Mathf.RoundToInt((_currentLevel * _baseLevelMaxPointsOnScreen) * maxMultiplier);
    }

    private void CheckLevelRequirements()
    {
        if (_levelPlayerPoints > _levelPoolPoints)
        {
            // spawn portal to new level
            // stop spawning asteroids?
        }
    }

    public void UpdatePlayerPoints(int pointsToAdd)
    {
        _levelPlayerPoints += pointsToAdd;
        CheckLevelRequirements();
    }

    public int GetMaxAsteroidPoints()
    {
        return _levelMaxPointsOnScreen;
    }
}
