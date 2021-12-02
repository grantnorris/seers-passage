using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManager : MonoBehaviour
{
    static List<Tip> tips = new List<Tip>() {
        new Tip(
            "Movement",
            new Dialogue(
                new string[] {
                    "Tip: Move around by dragging in either the up, down, left or right direction."
                },
                ""
            )
        ),
        new Tip(
            "Interactables",
            new Dialogue(
                new string[] {
                    "To interact with an object, drag again in it's direction."
                },
                ""
            )
        ),
        new Tip(
            "Light Sources",
            new Dialogue(
                new string[] {
                    "Tip: You'll be able to see much more of your surroundings when holding a lightsource,",
                    "they're worth searching for!",
                },
                ""
            )
        ),
        new Tip(
            "Keys",
            new Dialogue(
                new string[] {
                    "Tip: Keys are single-use consumable items which can be used to",
                    "reach blocked off areas!",
                },
                ""
            )
        ),
        new Tip(
            "Inventory",
            new Dialogue(
                new string[] {
                    "Tip: Items in your inventory can be seen in the",
                    "bottom right of the screen.",
                },
                ""
            )
        ),
        new Tip(
            "Life",
            new Dialogue(
                new string[] {
                    "Tip: Making too many steps in a dungeon will result in losing lives.",
                    "The number of remaining steps is indicated in the bottom left of the",
                    "screen, when this reaches zero, your last life will be lost!"
                },
                ""
            )
        ),
    };

    public static void DisplayTip(string tipName) {
        for (int i = 0; i < tips.Count; i++) {
            Tip tip = tips[i];

            if (tip.name != tipName) {
                continue;
            }

            // Don't display the same tip agains
            if (tip.displayed) {
                break;
            }

            DialogueManager.instance.StartDialogue(tip.dialogue);
            tip.displayed = true;
            break;
        }
    }
}

public class Tip {
    public string name; // This acts as an identifier so should be unique
    public Dialogue dialogue;
    public bool displayed = false;

    public Tip(string newName, Dialogue newDialogue) {
        name = newName;
        dialogue = newDialogue;
    }
}