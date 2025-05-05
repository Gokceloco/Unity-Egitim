using UnityEngine;

[System.Serializable]
public class StoreData
{
    public int lastLevelNo;
    public float healthRatio;
    public float[] playerPosition;
    public bool isSoundOn;

    public StoreData(int levelNo, float hRatio, Vector3 pPosition, bool soundOn)
    {
        lastLevelNo = levelNo;
        healthRatio = hRatio;
        isSoundOn = soundOn;

        playerPosition = new float[3];
        playerPosition[0] = pPosition.x;
        playerPosition[1] = pPosition.y;
        playerPosition[2] = pPosition.z;
    }
}
