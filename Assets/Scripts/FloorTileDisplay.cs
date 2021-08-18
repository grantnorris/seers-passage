using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileDisplay : TileDisplay
{
    public List<Sprite> topDetailSprites = new List<Sprite>();
    public bool hasDetails;

    public override void Initialise() {
        base.Initialise();
        CreateDetails();
    }

    void CreateDetails() {
        if (topDetailSprites.Count == 0) {
            return;
        }

        // if (Random.Range(0, 100) <= 50)  {
            for (int i = 0; i < 4; i++) {
                GameObject adjacentTile = null;
                float tileRotation = 0f;
                
                switch (i)
                {
                    // Up
                    case 0:
                    adjacentTile = tileUp;
                    tileRotation = 0f;
                    break;

                    // Right
                    case 1:
                    adjacentTile = tileRight;
                    tileRotation = -90f;
                    break;

                    // Down
                    case 2:
                    adjacentTile = tileDown;
                    tileRotation = -180f;
                    break;

                    // Left
                    case 3:
                    adjacentTile = tileLeft;
                    tileRotation = -270f;
                    break;
                }

                if (adjacentTile == null || adjacentTile.tag != "Wall") {
                    continue;
                }

                GameObject detail = new GameObject();
                Transform detailTransform = detail.transform;
                detail.name = "Floor Detail";
                detailTransform.SetParent(transform);
                detail.transform.localPosition = Vector3.zero;
                detail.transform.eulerAngles = new Vector3(0, 0, tileRotation);
                Sprite sprite = topDetailSprites[Random.Range(0, topDetailSprites.Count - 1)];
                detail.AddComponent<SpriteRenderer>().sprite = sprite;
            }
        // }
    }
}
