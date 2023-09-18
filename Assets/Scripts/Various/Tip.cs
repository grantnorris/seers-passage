public class Tip {
    public string name; // This acts as an identifier so should be unique
    public Dialogue dialogue;

    public Tip(string newName, Dialogue newDialogue) {
        name = newName;
        dialogue = newDialogue;
    }
}