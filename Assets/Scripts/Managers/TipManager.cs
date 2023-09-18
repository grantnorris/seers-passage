using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManager
{
    static List<string> displayedTips = new List<string>();
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
                    "The speech bubble appears when an interactable object is nearby",
                    "drag in its direction to interact with it."
                },
                ""
            )
        ),
        new Tip(
            "Light Sources",
            new Dialogue(
                new string[] {
                    "Tip: You'll be able to see much more of your surroundings when holding a light source,",
                    "they're worth searching for!",
                },
                ""
            )
        ),
        new Tip(
            "Keys",
            new Dialogue(
                new string[] {
                    "Tip: Keys are single-use consumable items which can be used to reach",
                    "blocked off areas!",
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

    // Display a tip dialogue by name and then add it to the list of displayed tips
    public static void DisplayTip(string tipName) {
        List<string> displayedTips = SaveSystem.DisplayedTips();

        if (displayedTips.Contains(tipName)) {
            return;
        }

        for (int i = 0; i < tips.Count; i++) {
            Tip tip = tips[i];

            if (tip.name != tipName) {
                continue;
            }

            DialogueManager.instance.StartDialogue(tip.dialogue);
            SaveSystem.AddTipToDisplayedList(tipName);
            break;
        }
    }

    // Get a tip by name
    public static Tip GetTip(string name) {
        foreach (Tip tip in tips) {
            if (tip.name != name) {
                continue;
            }

            return tip;
        }

        return null;
    }
}