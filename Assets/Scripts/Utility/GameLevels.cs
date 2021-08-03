using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLevels
{
    public static Level[] levels = new Level[] {
        Resources.Load<Level>("Levels/Floor 1"),
        Resources.Load<Level>("Levels/Floor 2"),
        Resources.Load<Level>("Levels/Floor 3"),
        Resources.Load<Level>("Levels/Floor 4"),
    };

    public static Level PreviousLevel(Level level) {
        Level prevLevel = null;

        for (int i = 0; i < levels.Length; i++) {
            if (levels[i] == level) {
                if (i - 1 < 0) {
                    return null;
                }

                prevLevel = levels[i - 1];
            }
        }

        return prevLevel;
    }

    public static Level NextLevel(Level level) {
        Level nextLevel = null;

        for (int i = 0; i < levels.Length; i++) {
            if (levels[i] == level) {
                if (i + 1 >= levels.Length) {
                    return null;
                }

                nextLevel = levels[i + 1];
            }
        }

        return nextLevel;
    }
}
