using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    // TO DO: Currently both the saveSystem and progressData scripts have multiple similar methods, these need to be optimised so it's less ambiguous.
    
    static string path = Path.Combine(Application.persistentDataPath, "data.sp");
    static ProgressData progressData = LoadProgress();

    public static void UpdateLevelScore(LevelScore score) {
        Logger.Send("Update level score.", "save");
        progressData.UpdateScore(score);
        SaveProgress(progressData);
    }

    // Retrieve a level score from the current saved data
    public static LevelScore LevelScore(Level level) {
        if (level == null) {
            return null;
        }

        return progressData.GetScore(level);
    }

    // Retrieve the total score of all levels combined from the current saved data
    public static int TotalScore() {
        return progressData.TotalScore();
    }

    // Flag a tip as displayed in the current saved data
    public static void AddTipToDisplayedList(string name) {
        Logger.Send($"Add tip to displayed list - name.", "save");

        progressData.AddTipToDisplayedList(name);
        SaveProgress(progressData);
    }

    // Get all displayed tips from the current saved data
    public static List<string> DisplayedTips() {
        return progressData.GetDisplayedTips();
    }

    // Retrieve all current saved data
    public static ProgressData ProgressData() {
        return progressData;
    }

    // Save current saved data
    static void SaveProgress(ProgressData data) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        progressData = data;

        Logger.Send("Saved data.", "save");
        data.Log();
    }

    // Load current saved data
    static ProgressData LoadProgress() {
        Logger.Send("Load progress.", "save");

        if (!File.Exists(path)) {
            Logger.Send("No data found to load, creating new data.", "save");
            return new ProgressData();
        }

        try {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ProgressData data = formatter.Deserialize(stream) as ProgressData;
            stream.Close();
            Logger.Send("Progress loaded.", "save");
            data.Log();

            return data;
        } catch (System.Exception) {
            Logger.Send("Could not load data correctly. Creating new data instead.", "save", "warning");
            
            return new ProgressData();
            throw;
        }
    }

    // Delete current saved data
    public static void DeleteProgress() {
        Logger.Send("Deleting saved data.", "save");
        File.Delete(path);
    }
}
