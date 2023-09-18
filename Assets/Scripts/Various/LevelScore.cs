using UnityEngine;

[System.Serializable]
public class LevelScore {
    public int hearts;
    public int steps;
    public float time;
    public string levelId;

    public LevelScore(int newHearts, int newSteps, float newTime, string newLevelId) {
        hearts = newHearts;
        steps = newSteps;
        time = newTime;
        levelId = newLevelId;
    }

    // Stringified version of score
    public string Score() {
        switch (hearts)
        {
            case 3 :
            return "Perfect!";
            
            case 2 :
            return "Great!";
            
            case 1 :
            return "Good";
        }

        return null;
    }

    // Whether or not a new score is better than an old one
    public static bool ScoreIsBetter(LevelScore newScore, LevelScore oldScore) {
        if (newScore.hearts < oldScore.hearts) {
            return false;
        }
        
        if (newScore.steps > oldScore.steps) {
            return false;
        }
        
        if (newScore.time > oldScore.time) {
            return false;
        }

        return true;
    }

    // Score time formatted for UI
    public string FormattedTime() {
        int mins = (int)(time / 60);
        int seconds = (int)(time - (mins * 60));
        return mins.ToString("00") + ":" + seconds.ToString("00");
    }
}