using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.Rendering;

public static class SaveSystem
{
    public static void SaveData(string dataName, int levelNo, float healthRatio,
        Vector3 playerPosition, bool isSoundOn)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + dataName + ".fun";
        FileStream stream = new FileStream(path, FileMode.Create);        

        StoreData data = new StoreData(levelNo, healthRatio, playerPosition, isSoundOn);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static StoreData LoadData(string dataName)
    {
        string path = Application.persistentDataPath + "/" + dataName + ".fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StoreData data = formatter.Deserialize(stream) as StoreData;

            if (data.lastLevelNo < 1)
            {
                data.lastLevelNo = 1;
            }
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

    public static string[] GetLoadFiles()
    {
        string path = Application.persistentDataPath + "/";
        DirectoryInfo levelDirectoryPath = new DirectoryInfo(path);
        FileInfo[] fileInfo = levelDirectoryPath.GetFiles("*.fun", SearchOption.AllDirectories);

        string[] fileNames = new string[fileInfo.Length];

        for (int i = 0; i < fileInfo.Length; i++)
        {
            var name = fileInfo[i].Name.Substring(0, fileInfo[i].Name.Length - 4);
            fileNames[i] = name;
        }
        return fileNames;
    }
}
