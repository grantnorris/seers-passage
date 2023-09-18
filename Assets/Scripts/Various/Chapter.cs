public class Chapter {
    public string name;
    public Level[] levels;
    public int scoreToUnlock = 0;

    public Chapter(string newName, Level[] newLevels, int newScoreToUnlock) {
        name = newName;
        levels = newLevels;
        scoreToUnlock = newScoreToUnlock;
    }

    // Whether or not the this chapter is unlocked based on required score
    public bool Unlocked() {
        return SaveSystem.TotalScore() >= scoreToUnlock;
    }
}