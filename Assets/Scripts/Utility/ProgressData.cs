using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    List<LevelScore> scores = new List<LevelScore>();
    List<string> displayedTips = new List<string>();

    public void UpdateScore(LevelScore score) {
        LevelScore curScore = null;

        for (int i = 0; i < scores.Count; i++) {
            if (scores[i].levelId != score.levelId) {
                continue;
            }

            curScore = scores[i];
            break;
        }

        if (curScore != null) {
            curScore = score;
        } else {
            scores.Add(score);
        }
    }

    public LevelScore GetScore(Level level) {
        if (scores.Count == 0 || level == null) {
            return null;
        }

        for (int i = 0; i < scores.Count; i++) {
            if (scores[i].levelId != level.id) {
                continue;
            }

            return scores[i];
        }

        return null;
    }

    public int TotalScore() {
        int score = 0;

        for (int i = 0; i < scores.Count; i++) {
            score += scores[i].hearts;
        }

        return score;
    }

    public void AddTipToDisplayedList(string name) {
        if (displayedTips.Contains(name)) {
            return;
        }

        displayedTips.Add(name);
    }

    public List<string> GetDisplayedTips() {
        return displayedTips;
    }

    public void Log() {
        Logger.Send("---- Saved data ----", "save");
        Logger.Send("---- Scores ----", "save");

        for (int i = 0; i < scores.Count; i++) {
            Logger.Send(scores[i].levelId + " = " + scores[i].Score(), "save");
        }

        Logger.Send("---- Displayed tips ----", "save");

        for (int i = 0; i < displayedTips.Count; i++) {
            Logger.Send(displayedTips[i], "save");
        }
    }
}