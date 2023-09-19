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

        // Add one sprite for each edge if the adjacent tile is a wall
        AddEdgeSpriteToList(map.GetTile(position.x, position.y + 1), topEdgeSprites);
        AddEdgeSpriteToList(map.GetTile(position.x, position.y - 1), bottomEdgeSprites);
        AddEdgeSpriteToList(map.GetTile(position.x - 1, position.y), leftEdgeSprites);
        AddEdgeSpriteToList(map.GetTile(position.x + 1, position.y), rightEdgeSprites);
    }

    // Add a sprite for a given edge to the list
    void AddEdgeSpriteToList(TileLocation adjacentTile, Sprite[] edgeSprites) {
        if (adjacentTile == null || adjacentTile.obj.tag != "Wall" || edgeSprites.Length == 0) {
            return;
        }

        sprites.Add(edgeSprites[Random.Range(0, edgeSprites.Length)]);
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
