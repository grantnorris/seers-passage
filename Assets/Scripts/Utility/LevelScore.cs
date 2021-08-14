using UnityEngine;

[System.Serializable]
public class LevelScore {
    public int hearts;
    public int steps;
    public float time;

    public LevelScore(int newHearts, int newSteps, float newTime) {
        hearts = newHearts;
        steps = newSteps;
        time = newTime;
    }

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
}