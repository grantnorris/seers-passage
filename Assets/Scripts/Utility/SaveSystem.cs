using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static string path = Path.Combine(Application.persistentDataPath, "data.sp");
    static ProgressData progressData = LoadProgress();
    static Chapter[] chapters = GameLevels.chapters;

    public static void UpdateLevelScore(LevelScore score) {
        Debug.Log("update level score");
        progressData.UpdateScore(score);

        SaveProgress(progressData);
    }

    public static LevelScore LevelScore(Level level) {
        if (level == null) {
            return null;
        }

        return progressData.GetScore(level);
    }

    public static ProgressData ProgressData() {
        return progressData;
    }

    // Save progress
    static void SaveProgress(ProgressData data) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        progressData = data;
    }

    // Load progress
    static ProgressData LoadProgress() {
        Debug.Log("load progress");

        if (!File.Exists(path)) {
            Debug.Log("no data found to load, create new data");
            return new ProgressData();
        }

        try {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ProgressData data = formatter.Deserialize(stream) as ProgressData;
            stream.Close();
            Debug.Log("progress loaded");
            return data;
        } catch (System.Exception) {
            Debug.LogWarning("Could NOT load data correctly. Creating new data instead.");
            return new ProgressData();
            throw;
        }
    }

    public static void DeleteProgress() {
        Debug.Log("delete saved data");
        File.Delete(path);
        progressData = new ProgressData();
    }
}
