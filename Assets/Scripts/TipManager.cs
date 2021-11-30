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
                    "Tip: Some objects can be interacted with,",
                    "this will be indicated by an exclaimation mark above your head.",
                    "To interact with an object, drag in it's direction."
                },
                ""
            )
        ),
        new Tip(
            "Light Sources",
            new Dialogue(
                new string[] {
                    "Tip: You'll be able to see much more of your surroundings when holding a lightsource, they're worth searching for!",
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

            print("play " + tipName + " dialouge");
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