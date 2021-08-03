using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public LevelScore[] scores;

    public ProgressData() {
        Level[] levels = GameLevels.levels;
        LevelScore[] scores = new LevelScore[levels.Length];
    }
}

public class LevelScore {
    public int hearts;
    public int steps;
    public float time;

    public LevelScore(int newHearts, int newSteps, float newTime) {
        hearts = newHearts;
        steps = newSteps;
        time = newTime;
    }
}