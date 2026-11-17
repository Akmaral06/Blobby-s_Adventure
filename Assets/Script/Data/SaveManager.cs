using UnityEngine;
using System.IO;

public static class SaveManager
{
    static string path => Application.persistentDataPath + "/save.json";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static SaveData Load()
    {
        try
        {
            if (!File.Exists(path)) return new SaveData();
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            if (data == null) return new SaveData();
            return data;
        }
        catch
        {
            return new SaveData();
        }
    }
}