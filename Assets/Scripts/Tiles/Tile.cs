using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Tile", menuName="Tile")]
public class Tile : ScriptableObject
{
    public new string name;
    public string type;
    public Sprite mainSprite;
    public Sprite lightSourceUp;
    public Sprite lightSourceDown;
    public Sprite lightSourceLeft;
    public Sprite lightSourceRight;
}
