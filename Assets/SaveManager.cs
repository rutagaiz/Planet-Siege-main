/*
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveManager
{
    private static string path = Application.persistentDataPath + "/gamedata.json";

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("💾 Auto-saved!");
    }

    public static GameData Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<GameData>(json);
        }
        return null;
    }

    public static void Delete()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("🗑️ Save deleted.");
        }
    }
    public static bool HasSave()
    {
        return File.Exists(Path.Combine(Application.persistentDataPath, path));
    }
    
    public static void DeleteSave()
    {
        //string path = Path.Combine(Application.persistentDataPath, path);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("🗑️ Save file deleted.");
        }
    }
}
*/
