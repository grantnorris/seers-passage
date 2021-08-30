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

    Sprite sprite;
    SpriteRenderer rend;

    void Awake() {
        rend = GetComponent<SpriteRenderer>();
    }

    void Start() {
        Initialise();
    }

    // Initialise
    public void Initialise() {
        if (neutralSprites.Length > 0) {
            sprite = neutralSprites[Random.Range(0, neutralSprites.Length)];

            if (rend != null) {
                rend.sprite = sprite;
            }

            Map map = Map.instance;
            Vector2 position  = transform.position;

            GameObject tileUp    = map.GetTile(position.x, position.y + 1);
            GameObject tileDown  = map.GetTile(position.x, position.y - 1);
            GameObject tileLeft  = map.GetTile(position.x - 1, position.y);
            GameObject tileRight = map.GetTile(position.x + 1, position.y);

            List<Sprite> sprites = new List<Sprite>();

            if (tileUp != null && tileUp.tag == "Wall" && topEdgeSprites.Length > 0) {
                sprites.Add(topEdgeSprites[Random.Range(0, topEdgeSprites.Length)]);
            }

            if (tileDown != null && tileDown.tag == "Wall" && bottomEdgeSprites.Length > 0) {
                sprites.Add(bottomEdgeSprites[Random.Range(0, bottomEdgeSprites.Length)]);
            }

            if (tileLeft != null && tileLeft.tag == "Wall" && leftEdgeSprites.Length > 0) {
                sprites.Add(leftEdgeSprites[Random.Range(0, leftEdgeSprites.Length)]);
            }
            
            if (tileRight != null && tileRight.tag == "Wall" && rightEdgeSprites.Length > 0) {
                sprites.Add(rightEdgeSprites[Random.Range(0, rightEdgeSprites.Length)]);
            }

            foreach (Sprite sprite in sprites) {
                GameObject layer = new GameObject("Sprite Layer");
                layer.transform.SetParent(transform);
                layer.transform.localPosition = new Vector2(0, 0);
                layer.AddComponent<SpriteRenderer>().sprite = sprite;
                layer.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }
    }
}
