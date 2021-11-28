using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLevels
{
    public static Chapter[] chapters = new Chapter[] {
        new Chapter(
            "Chapter One",
            new Level[] {
                Resources.Load<Level>("Levels/Floor 1"),
                Resources.Load<Level>("Levels/Floor 2"),
                Resources.Load<Level>("Levels/Floor 3"),
                Resources.Load<Level>("Levels/Floor 4"),
            }
        ),
        new Chapter(
            "Chapter Two",
            new Level[] {
                Resources.Load<Level>("Levels/Floor 1"),
                Resources.Load<Level>("Levels/Floor 2"),
            }
        )
    };

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

    public static Level NextLevel(Level level) {
        for (int c = 0; c < chapters.Length; c++) {
            for (int l = 0; l < chapters[c].levels.Length; l++) {
                if (level != chapters[c].levels[l]) {
                    continue;
                }

                if (l == chapters[c].levels.Length - 1 && c == chapters.Length - 1) {
                    return null;
                } else if (l == chapters[c].levels.Length - 1) {
                    Chapter nextChapter = chapters[c - 1];

                    if (nextChapter.levels.Length == 0) {
                        return null;
                    }

                    return nextChapter.levels[0];
                }

                return chapters[c].levels[l + 1];
            }
        }

        return null;
    }
}

public class Chapter {
    public string name;
    public Level[] levels;

    public Chapter(string newName, Level[] newLevels) {
        name = newName;
        levels = newLevels;
    }
}