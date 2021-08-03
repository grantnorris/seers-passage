using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static string path = Path.Combine(Application.persistentDataPath, "data.sp");

    // Save progress
    public static void SaveProgress(ProgressData data) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("progress saved");
    }

    // Load progress
    public static ProgressData LoadProgress() {
        if (!File.Exists(path)) {
            Debug.Log("no data found to load");
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);
        ProgressData data = formatter.Deserialize(stream) as ProgressData;
        stream.Close();
        Debug.Log("loaded data, score = " + data.score);
        return data;
    }
}
