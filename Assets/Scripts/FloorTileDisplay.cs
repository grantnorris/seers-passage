using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileDisplay : TileDisplay
{
    [SerializeField]
    Sprite[] puddleSprites;
    [SerializeField]
    List<Sprite> topDetailSprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> rightDetailSprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> downDetailSprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> leftDetailSprites = new List<Sprite>();
    [HideInInspector]
    public bool hasPuddle = false;
    string adjacentPuddles = "";
    [SerializeField]
    bool hasDetails;
    [HideInInspector]
    public bool hasTopDetails;
    [HideInInspector]
    public bool hasRightDetails;
    [HideInInspector]
    public bool hasBottomDetails;
    [HideInInspector]
    public bool hasLeftDetails;
    FloorTileDisplay tileUpDisplay;
    FloorTileDisplay tileRightDisplay;
    FloorTileDisplay tileDownDisplay;
    FloorTileDisplay tileLeftDisplay;

    public override void Initialise() {
        base.Initialise();
        tileUpDisplay = tileUp.GetComponent<FloorTileDisplay>();
        tileRightDisplay = tileRight.GetComponent<FloorTileDisplay>();
        tileDownDisplay = tileDown.GetComponent<FloorTileDisplay>();
        tileLeftDisplay = tileLeft.GetComponent<FloorTileDisplay>();
        hasDetails = false;
        hasPuddle = false;
        AssignDetails();
        StartCoroutine(CreateDetails());
    }

    // Determine which details should be present, if any
    void AssignDetails() {
        // Chance to create cobwebs
        if (Random.Range(0, 100) <= 25)  {
            for (int i = 0; i < 4; i++) {
                GameObject adjacentTile = null;
                List<Sprite> sprites = new List<Sprite>();
                
                switch (i)
                {
                    // Up
                    case 0:
                    adjacentTile = tileUp;
                    sprites = topDetailSprites;
                    break;

                    // Right
                    case 1:
                    adjacentTile = tileRight;
                    sprites = rightDetailSprites;
                    break;

                    // Down
                    case 2:
                    adjacentTile = tileDown;
                    sprites = downDetailSprites;
                    break;

                    // Left
                    case 3:
                    adjacentTile = tileLeft;
                    sprites = leftDetailSprites;
                    break;
                }

                if (sprites.Count == 0 || adjacentTile == null || adjacentTile.tag != "Wall") {
                    continue;
                }

                hasDetails = true;
            
                switch (i)
                {
                    // Up
                    case 0:
                    hasTopDetails = true;
                    break;

                    // Right
                    case 1:
                    hasRightDetails = true;
                    break;

                    // Down
                    case 2:
                    hasBottomDetails = true;
                    break;

                    // Left
                    case 3:
                    hasLeftDetails = true;
                    break;
                }            
            }
            // Chance to create puddle
        } else if (Random.Range(0, 100) <= 25) {
            hasPuddle = true;
        }
    }

    // Create various details
    IEnumerator CreateDetails() {
        if (!hasDetails && !hasPuddle) {
            yield break;
        }

        yield return null;

        if (hasDetails) {
            for (int i = 0; i < 4; i++) {
                Sprite sprite = DetailSprite(i);

                if (sprite == null) {
                    continue;
                }

                GameObject detail = new GameObject();
                Transform detailTransform = detail.transform;
                detail.name = "Floor Detail";
                detailTransform.SetParent(transform);
                detailTransform.localPosition = Vector3.zero;
                detail.AddComponent<SpriteRenderer>().sprite = sprite;
            }
        } else {
            Sprite sprite = PuddleSprite();

            if (sprite == null) {
                yield break;
            }

            GameObject puddle = new GameObject();
            Transform detailTransform = puddle.transform;
            puddle.name = "Puddle";
            detailTransform.SetParent(transform);
            detailTransform.localPosition = Vector3.zero;
            SpriteRenderer rend = puddle.AddComponent<SpriteRenderer>();
            SpriteMask spriteMask = puddle.AddComponent<SpriteMask>();
            rend.sprite = sprite;
            rend.sortingOrder = -2;
            spriteMask.sprite = sprite;
        }
    }

    // Detail sprite based on edge and adjacent tiles
    Sprite DetailSprite(int edge) {
        Sprite sprite = null;

        switch (edge)
        {
            // Up
            case 0:
            if (!hasTopDetails) {
                break;
            }

            if (tileLeftDisplay != null && tileLeftDisplay.hasTopDetails && tileRightDisplay != null && tileRightDisplay.hasTopDetails) {
                sprite = topDetailSprites[2];
            } else if (tileLeftDisplay != null && tileLeftDisplay.hasTopDetails) {
                sprite = topDetailSprites[3];
            } else if (tileRightDisplay != null && tileRightDisplay.hasTopDetails) {
                sprite = topDetailSprites[1];
            } else {
                sprite = topDetailSprites[0];
            }
            break;

            // Right
            case 1:
            if (!hasRightDetails) {
                break;
            }

            if (tileUpDisplay != null && tileUpDisplay.hasRightDetails && tileDownDisplay != null && tileDownDisplay.hasRightDetails) {
                sprite = rightDetailSprites[2];
            } else if (tileDownDisplay != null && tileDownDisplay.hasRightDetails) {
                sprite = rightDetailSprites[3];
            } else if (tileUpDisplay != null && tileUpDisplay.hasRightDetails) {
                sprite = rightDetailSprites[1];
            } else {
                sprite = rightDetailSprites[0];
            }
            break;

            // Down
            case 2:
            if (!hasBottomDetails) {
                break;
            }

            if (tileLeftDisplay != null && tileLeftDisplay.hasBottomDetails && tileRightDisplay != null && tileRightDisplay.hasBottomDetails) {
                sprite = downDetailSprites[2];
            } else if (tileLeftDisplay != null && tileLeftDisplay.hasBottomDetails) {
                sprite = downDetailSprites[3];
            } else if (tileRightDisplay != null && tileRightDisplay.hasBottomDetails) {
                sprite = downDetailSprites[1];
            } else {
                sprite = downDetailSprites[0];
            }
            break;

            // Left
            case 3:
            if (!hasLeftDetails) {
                break;
            }

            if (tileUpDisplay != null && tileUpDisplay.hasLeftDetails && tileDownDisplay != null && tileDownDisplay.hasLeftDetails) {
                sprite = leftDetailSprites[2];
            } else if (tileDownDisplay != null && tileDownDisplay.hasLeftDetails) {
                sprite = leftDetailSprites[3];
            } else if (tileUpDisplay != null && tileUpDisplay.hasLeftDetails) {
                sprite = leftDetailSprites[1];
            } else {
                sprite = leftDetailSprites[0];
            }
            break;
        }

        return sprite;
    }

    Sprite PuddleSprite() {
        Sprite sprite = null;
        adjacentPuddles = "";

        adjacentPuddles = tileUpDisplay != null && tileUpDisplay.hasPuddle ? "1" : "0";
        adjacentPuddles += tileRightDisplay != null && tileRightDisplay.hasPuddle ? "1" : "0";
        adjacentPuddles += tileDownDisplay != null && tileDownDisplay.hasPuddle ? "1" : "0";
        adjacentPuddles += tileLeftDisplay != null && tileLeftDisplay.hasPuddle ? "1" : "0";

        switch (adjacentPuddles)
        {
            case "0000":
            // Lower chance of displayer unconnected puddles
            if (Random.Range(0, 100) <= 25) {
                sprite = puddleSprites[0];
            }
            break;

            case "1000":
            sprite = puddleSprites[1];
            break;
            
            case "0100":
            sprite = puddleSprites[2];
            break;

            case "0010":
            sprite = puddleSprites[3];
            break;

            case "0001":
            sprite = puddleSprites[4];
            break;
            
            case "0110":
            sprite = puddleSprites[5];
            break;

            case "0011":
            sprite = puddleSprites[6];
            break;

            case "1100":
            sprite = puddleSprites[7];
            break;

            case "1001":
            sprite = puddleSprites[8];
            break;

            case "1111":
            sprite = puddleSprites[9];
            break;

            case "1110":
            sprite = puddleSprites[10];
            break;

            case "1101":
            sprite = puddleSprites[11];
            break;

            case "1011":
            sprite = puddleSprites[12];
            break;

            case "0111":
            sprite = puddleSprites[13];
            break;

            case "1010":
            sprite = puddleSprites[14];
            break;

            case "0101":
            sprite = puddleSprites[15];
            break;

        }

        return sprite;
    }
}
