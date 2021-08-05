using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public LevelScore[] scores;

    public ProgressData() {
        Level[] levels = GameLevels.levels;
        scores = new LevelScore[levels.Length];
    }
}

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
}