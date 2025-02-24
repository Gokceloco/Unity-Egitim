using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Level level1;
    private Level _currentLevel;
    public void CreateLevel()
    {
        _currentLevel = Instantiate(level1);
        _currentLevel.transform.position = Vector3.zero;
    }

    public void DeleteCurrentLevel()
    {
        if (_currentLevel)
        {
            Destroy(_currentLevel.gameObject);
        }
    }
}
