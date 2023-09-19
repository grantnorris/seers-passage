using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingTile : MonoBehaviour
{
    public Sprite[] neutralSprites;
    public Sprite[] topEdgeSprites;
    public Sprite[] bottomEdgeSprites;
    public Sprite[] leftEdgeSprites;
    public Sprite[] rightEdgeSprites;

    List<Sprite> sprites = new List<Sprite>();

    Sprite sprite;
    SpriteRenderer rend;

    void Awake() {
        rend = GetComponent<SpriteRenderer>();
    }

    void Start() {
        GetSprites();
        CreateSprites();
    }

    // Set tile sprites based on surrounding walls 
    void GetSprites() {
        if (neutralSprites.Length == 0) {
            return;
        }

        sprite = neutralSprites[Random.Range(0, neutralSprites.Length)];

        if (rend != null) {
            rend.sprite = sprite;
        }

        Map map = Map.instance;
        Vector2 position  = transform.position;

        TileLocation tileUp = map.GetTile(position.x, position.y + 1);
        TileLocation tileDown = map.GetTile(position.x, position.y - 1);
        TileLocation tileLeft = map.GetTile(position.x - 1, position.y);
        TileLocation tileRight = map.GetTile(position.x + 1, position.y);

        if (tileUp != null && tileUp.obj.tag == "Wall" && topEdgeSprites.Length > 0) {
            sprites.Add(topEdgeSprites[Random.Range(0, topEdgeSprites.Length)]);
        }

        if (tileDown != null && tileDown.obj.tag == "Wall" && bottomEdgeSprites.Length > 0) {
            sprites.Add(bottomEdgeSprites[Random.Range(0, bottomEdgeSprites.Length)]);
        }

        if (tileLeft != null && tileLeft.obj.tag == "Wall" && leftEdgeSprites.Length > 0) {
            sprites.Add(leftEdgeSprites[Random.Range(0, leftEdgeSprites.Length)]);
        }
        
        if (tileRight != null && tileRight.obj.tag == "Wall" && rightEdgeSprites.Length > 0) {
            sprites.Add(rightEdgeSprites[Random.Range(0, rightEdgeSprites.Length)]);
        }
    }

    // Create gameobjects for each sprite
    void CreateSprites() {
        foreach (Sprite sprite in sprites) {
            GameObject layer = new GameObject("Sprite Layer");
            layer.transform.SetParent(transform);
            layer.transform.localPosition = new Vector2(0, 0);
            layer.AddComponent<SpriteRenderer>().sprite = sprite;
            layer.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }
}
