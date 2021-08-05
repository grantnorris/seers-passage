using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static string path = Path.Combine(Application.persistentDataPath, "data.sp");
    static ProgressData progressData = LoadProgress();
    static Level[] levels = GameLevels.levels;

    public static void UpdateLevelScore(Level level, LevelScore score) {
        for (int i = 0; i < levels.Length; i++) {
            if (levels[i] == level) {
                progressData.scores[i] = score;
            }
        }

        SaveProgress(progressData);
    }

    public static LevelScore LevelScore(Level level) {
        for (int i = 0; i < levels.Length; i++) {
            if (levels[i] == level) {
                return progressData.scores[i];
            }
        }

        return null;
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
        if (!File.Exists(path)) {
            Debug.Log("no data found to load, create new data");
            return new ProgressData();
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);
        ProgressData data = formatter.Deserialize(stream) as ProgressData;
        stream.Close();
        Debug.Log("progress loaded");
        return data;
    }
}
