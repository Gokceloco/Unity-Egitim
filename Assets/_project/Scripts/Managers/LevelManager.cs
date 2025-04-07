using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public Level level1Prefab;

    private Level _currentLevel;
    public void CreateLevel()
    {
        _currentLevel = Instantiate(level1Prefab);
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
}
