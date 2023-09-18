public class Chapter {
    public string name;
    public Level[] levels;
    public int scoreToUnlock = 0;

    public Chapter(string newName, Level[] newLevels, int newScoreToUnlock) {
        name = newName;
        levels = newLevels;
        scoreToUnlock = newScoreToUnlock;
    }

    public bool Unlocked() {
        return SaveSystem.TotalScore() >= scoreToUnlock;
    }
}