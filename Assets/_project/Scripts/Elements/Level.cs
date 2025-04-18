using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    private List<Enemy> _enemies = new List<Enemy>();
    public void StartLevel(Player player)
    {
        var collectables = GetComponentsInChildren<Collectable>();
        for (int i = 0; i < collectables.Length; i++)
        {
            collectables[i].StartCollectable(i * .1f);
        }
        _enemies = GetComponentsInChildren<Enemy>().ToList();
        foreach (var e in _enemies)
        {
            e.StartEnemy(player);
        }
    }
    public void StopEnemies()
    {
        foreach (var e in _enemies)
        {
            e.StopEnemy();
        }
    }
}
