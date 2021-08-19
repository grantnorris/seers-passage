using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileDisplay : TileDisplay
{
    [SerializeField]
    List<Sprite> topDetailSprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> rightDetailSprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> downDetailSprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> leftDetailSprites = new List<Sprite>();
    [HideInInspector]
    public bool hasTopDetails;
    [HideInInspector]
    public bool hasRightDetails;
    [HideInInspector]
    public bool hasBottomDetails;
    [HideInInspector]
    public bool hasLeftDetails;

    public override void Initialise() {
        base.Initialise();
        AssignDetails();
        CreateDetails();
        StartCoroutine(CreateDetails());
    }

    // Determine which details should be present, if any
    void AssignDetails() {
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
        }
    }

    IEnumerator CreateDetails() {
        yield return null;
        FloorTileDisplay tileUpDisplay = tileUp.GetComponent<FloorTileDisplay>();
        FloorTileDisplay tileRightDisplay = tileRight.GetComponent<FloorTileDisplay>();
        FloorTileDisplay tileDownDisplay = tileDown.GetComponent<FloorTileDisplay>();
        FloorTileDisplay tileLeftDisplay = tileLeft.GetComponent<FloorTileDisplay>();

        for (int i = 0; i < 4; i++) {
            Sprite sprite = null;

            switch (i)
            {
                // Up
                case 0:
                if (!hasTopDetails) {
                    continue;
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
    }
}
