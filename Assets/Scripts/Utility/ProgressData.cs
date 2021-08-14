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