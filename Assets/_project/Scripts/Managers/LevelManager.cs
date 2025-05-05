using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public List<Level> levels;

    private Level _currentLevel;

    public int levelNo;
    public void CreateLevel()
    {
        _currentLevel = Instantiate(levels[levelNo - 1]);
        _currentLevel.transform.position = Vector3.zero;
        _currentLevel.StartLevel(gameDirector.player);
    }

    public void DeleteCurrentLevel()
    {
        if (_currentLevel)
        {
            Destroy(_currentLevel.gameObject);
        }
    }

    public void SetParentToMap(Transform obj)
    {
        obj.SetParent(_currentLevel.transform);
    }

    public void StopEnemies()
    {
        _currentLevel.StopEnemies();
    }
}
