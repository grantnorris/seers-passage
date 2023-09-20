using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLevels
{   
    // List of playable chapters and their associated levels
    public static Chapter[] chapters = new Chapter[] {
        new Chapter(
            "Chapter One",
            new Level[] {
                Resources.Load<Level>("Levels/Floor 1"),
                Resources.Load<Level>("Levels/Floor 2"),
                Resources.Load<Level>("Levels/Floor 3"),
            },
            0
        ),
        new Chapter(
            "Chapter Two",
            new Level[] {
                Resources.Load<Level>("Levels/Floor 4"),
                Resources.Load<Level>("Levels/Floor 5"),
                Resources.Load<Level>("Levels/Floor 6"),
            },
            50
        ),
        new Chapter(
            "Chapter Three",
            new Level[] {
                Resources.Load<Level>("Levels/Floor 7"),
            },
            100
        ),
    };

    // Previous of a given level in the chapter list
    public static Level PreviousLevel(Level level) {
        for (int c = 0; c < chapters.Length; c++) {
            for (int l = 0; l < chapters[c].levels.Length; l++) {
                if (level != chapters[c].levels[l]) {
                    continue;
                }

                if (l == 0 && c == 0) {
                    return null;
                } else if (l == 0) {
                    Chapter prevChapter = chapters[c - 1];

                    if (prevChapter.levels.Length == 0) {
                        return null;
                    }

                    return prevChapter.levels[prevChapter.levels.Length - 1];
                }

                return chapters[c].levels[l - 1];
            }
        }

        return null;
    }

    // Next of a given level in the chapter list
    public static Level NextLevel(Level level) {
        for (int c = 0; c < chapters.Length; c++) {
            for (int l = 0; l < chapters[c].levels.Length; l++) {
                if (level != chapters[c].levels[l]) {
                    continue;
                }

                if (l == chapters[c].levels.Length - 1 && c == chapters.Length - 1) {
                    // This is the final level in the final chapter, so there's nothing to return
                    return null;
                } else if (l == chapters[c].levels.Length - 1) {
                    if (c == chapters.Length - 1) {
                        // This is the last chapter, there is no next level
                        return null;
                    }

                    // This is the final level in the current chapter, so return the first level of the next chapter
                    Chapter nextChapter = chapters[c + 1];

                    if (nextChapter.levels.Length == 0) {
                        return null;
                    }

                    return nextChapter.levels[0];
                }

                // Return the next level in this chapter
                return chapters[c].levels[l + 1];
            }
        }

        return null;
    }
}