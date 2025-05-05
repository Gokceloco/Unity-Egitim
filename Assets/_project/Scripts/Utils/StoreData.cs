using UnityEngine;

[System.Serializable]
public class StoreData
{
    public int level;
    public float healthRatio;
    public float[] playerPosition;

    public StoreData(int levelNo, float hRatio, Vector3 pPosition)
    {
        level = levelNo;
        healthRatio = hRatio;

        playerPosition = new float[3];
        playerPosition[0] = pPosition.x;
        playerPosition[1] = pPosition.y;
        playerPosition[2] = pPosition.z;
    }
}
