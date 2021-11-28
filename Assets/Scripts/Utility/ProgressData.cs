using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public List<LevelScore> scores = new List<LevelScore>();

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
}