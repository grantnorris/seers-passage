using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static string path = Path.Combine(Application.persistentDataPath, "data.sp");
    static ProgressData progressData = LoadProgress();

    public static void UpdateLevelScore(LevelScore score) {
        Logger.Send("Update level score", "save");
        progressData.UpdateScore(score);
        SaveProgress(progressData);
    }

    public static LevelScore LevelScore(Level level) {
        if (level == null) {
            return null;
        }

        return progressData.GetScore(level);
    }

    public static int TotalScore() {
        return progressData.TotalScore();
    }

    public static void AddTipToDisplayedList(string name) {
        Logger.Send("Add tip to displayed list - " + name, "save");

        progressData.AddTipToDisplayedList(name);
        SaveProgress(progressData);
    }

    public static List<string> DisplayedTips() {
        return progressData.GetDisplayedTips();
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

        Logger.Send("Saved data", "save");
        data.Log();
    }

    // Load progress
    static ProgressData LoadProgress() {
        Logger.Send("Load progress", "save");

        if (!File.Exists(path)) {
            Logger.Send("No data found to load, creating new data", "save");
            return new ProgressData();
        }

        try {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ProgressData data = formatter.Deserialize(stream) as ProgressData;
            stream.Close();
            Logger.Send("Progress loaded", "save");
            data.Log();

            return data;
        } catch (System.Exception) {
            Debug.LogWarning("Could not load data correctly. Creating new data instead.");
            return new ProgressData();
            throw;
        }
    }

    public static void DeleteProgress() {
        Logger.Send("Deleting saved data", "save");
        File.Delete(path);
        // progressData = new ProgressData();
    }
}
