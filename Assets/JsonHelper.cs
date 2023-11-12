using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonHelper : MonoBehaviour
{
    static string path = Application.persistentDataPath + "/RoundsData.json";
    public static RoundsData GetRoundsData()
    {
        string jsonData = File.ReadAllText(path);
        return JsonUtility.FromJson<RoundsData>(jsonData);
    }
    public static void WriteRoundDataToJson()
    {
        RoundsData data = new RoundsData();
        string json = JsonUtility.ToJson(data, false);
        File.WriteAllText(path, json);
    }
}
