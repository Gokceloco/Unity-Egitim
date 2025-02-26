using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    public void StartLevel()
    {
        var collectables = GetComponentsInChildren<Collectable>();
        for (int i = 0; i < collectables.Length; i++)
        {
            collectables[i].StartCollectable(i * .1f);
        }
    }
}
