using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileDisplay : TileDisplay
{
    // TODO: there are a lot of ambiguous loops and switch statements here that could be tidied but would likely require larger refactoring

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
    bool hasDetails = false;
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
        
        SetAdjacentTileReferences();
        AssignDetails();
        CreateDetails();
    }

    // Retrieve references to adjacent tiles
    void SetAdjacentTileReferences() {
        tileUpDisplay = (tileUp != null ? tileUp.obj.GetComponent<FloorTileDisplay>() : null);
        tileRightDisplay = (tileRight != null ? tileRight.obj.GetComponent<FloorTileDisplay>() : null);
        tileDownDisplay = (tileDown != null ? tileDown.obj.GetComponent<FloorTileDisplay>() : null);
        tileLeftDisplay = (tileLeft != null ? tileLeft.obj.GetComponent<FloorTileDisplay>() : null);
    }

    // Determine which details should be present based on adjacent tiles and RNG
    // Details include details or puddles (never both)
    void AssignDetails() {
        // Chance to create details
        if (CanHaveDetails())  {
            MaybeAssignDetailSprites();
        } else {
            MaybeAssignPuddle();
        }
    }

    // Whether or not the tile can have details, based on chance
    bool CanHaveDetails() {
        int percentChanceToHaveDetails = 25;

        return Random.Range(0, 100) <= percentChanceToHaveDetails;
    }

    // Assign detail sprites if adjacent tiles are walls
    void MaybeAssignDetailSprites() {
        for (int i = 0; i < 4; i++) {
            TileLocation adjacentTile = null;
            List<Sprite> sprites = new List<Sprite>();
            
            switch (i) {
                case 0:
                    adjacentTile = tileUp;
                    break;
                case 1:
                    adjacentTile = tileRight;
                    break;
                case 2:
                    adjacentTile = tileDown;
                    break;
                case 3:
                    adjacentTile = tileLeft;
                    break;
            }

            if (adjacentTile == null || adjacentTile.obj.tag != "Wall") {
                // Ignore this face of the tile if the adjacent tile isn't a wall
                continue;
            }

            hasDetails = true;
        
            switch (i) {
                case 0:
                    hasTopDetails = true;
                    sprites = topDetailSprites;
                    break;
                case 1:
                    hasRightDetails = true;
                    sprites = rightDetailSprites;
                    break;
                case 2:
                    hasBottomDetails = true;
                    sprites = downDetailSprites;
                    break;
                case 3:
                    hasLeftDetails = true;
                    sprites = leftDetailSprites;
                    break;
            }            
        }
    }

    // Assign puddle based on chance
    void MaybeAssignPuddle() {
        // Chance to create puddle if no wobwebs are present
        // More likely if adjacent tiles have puddles
        int percentChanceToHavePuddle = AdjacentTilesHavePuddles() ? 80 : 40;

        if (Random.Range(0, 100) <= percentChanceToHavePuddle) {
            hasPuddle = true;
        }
    }

    // Whether or not puddles are present in adjacent tiles
    bool AdjacentTilesHavePuddles() {
        if (tileUpDisplay != null && tileUpDisplay.hasPuddle) {
            return true;
        }
        
        if (tileDownDisplay != null && tileDownDisplay.hasPuddle) {
            return true;
        }

        if (tileLeftDisplay != null && tileLeftDisplay.hasPuddle) {
            return true;
        }

        if (tileRightDisplay != null && tileRightDisplay.hasPuddle) {
            return true;
        }

        return false;
    }

    // Create various details
    void CreateDetails() {
        if (!hasDetails && !hasPuddle) {
            return;
        }

        if (hasDetails) {
            CreateDetailSprites();
        } else if (hasPuddle) {
            CreatePuddleSprite();
        }
    }

    // Create and set up detail sprite gameobjects
    void CreateDetailSprites() {
        for (int i = 0; i < 4; i++) {
            Sprite sprite = DetailSprite(i);

            if (sprite == null) {
                continue;
            }

            GameObject detail = new GameObject();
            Transform detailTransform = detail.transform;
            
            detail.name = "Floor detail " + gameObject.name;
            detailTransform.SetParent(transform);
            detailTransform.localPosition = Vector3.zero;
            detail.AddComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    // Create and set up puddle sprite gameobject
    void CreatePuddleSprite() {
        Sprite sprite = PuddleSprite();

        if (sprite == null) {
            return;
        }

        GameObject puddle = new GameObject();
        Transform detailTransform = puddle.transform;
        SpriteRenderer rend = puddle.AddComponent<SpriteRenderer>();
        SpriteMask spriteMask = puddle.AddComponent<SpriteMask>();

        puddle.name = "Puddle " + gameObject.name;
        detailTransform.SetParent(transform);
        detailTransform.localPosition = Vector3.zero;
        rend.sprite = sprite;
        rend.sortingOrder = -2;
        spriteMask.sprite = sprite;
    }

    // Detail sprite based on edge and adjacent tiles
    Sprite DetailSprite(int edge) {
        Sprite sprite = null;

        switch (edge) {
            case 0: // Up
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
            case 1: // Right
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
            case 2: // Down
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
            case 3: // Left
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

    // Retrieve puddle sprite based on surrounding puddle tiles
    Sprite PuddleSprite() {
        Sprite sprite = null;
        adjacentPuddles = tileUpDisplay != null && tileUpDisplay.hasPuddle ? "1" : "0";
        adjacentPuddles += tileRightDisplay != null && tileRightDisplay.hasPuddle ? "1" : "0";
        adjacentPuddles += tileDownDisplay != null && tileDownDisplay.hasPuddle ? "1" : "0";
        adjacentPuddles += tileLeftDisplay != null && tileLeftDisplay.hasPuddle ? "1" : "0";

        switch (adjacentPuddles) {
            case "0000":
                // Don't display unconnected singular puddles
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
