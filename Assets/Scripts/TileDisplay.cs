﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileDisplay : MonoBehaviour
{
    public Tile[] neutralTiles;
    public Tile[] downFacingTiles;
    public bool acceptsLight = false;
    public Tile tile;
    public GameObject lightingUp;
    public GameObject lightingDown;
    public GameObject lightingLeft;
    public GameObject lightingRight;

    protected GameObject tileUp;
    protected GameObject tileDown;
    protected GameObject tileLeft;
    protected GameObject tileRight;

    SpriteRenderer rend;
    bool isDownFacing = false;

    // Start is called before the first frame update
    void Start()
    {
        if (rend == null) {
            rend = GetComponent<SpriteRenderer>();
        }

        Initialise();
    }

    // Initialise by assigning lightsources and tile
    public virtual void Initialise() {
        if (rend == null) {
            rend = GetComponent<SpriteRenderer>();
        }

        SurroundingTiles();
        EnableLightSources();
        AssignTile();
    }

    void SurroundingTiles() {
        Vector2 position  = transform.position;
        BuildMap buildMap = BuildMap.instance;
        tileUp = buildMap.GetTileByLocation(position.x, position.y + 1);
        tileDown = buildMap.GetTileByLocation(position.x, position.y - 1);
        tileLeft = buildMap.GetTileByLocation(position.x - 1, position.y);
        tileRight = buildMap.GetTileByLocation(position.x + 1, position.y);
    }

    // Enable directional light sources based on the surrounding tiles
    void EnableLightSources() {
        if (!acceptsLight || gameObject.tag == "Floor") {
            return;
        }

        // Check to see if a wall is above
        if (tileUp != null && tileUp.tag == "Floor") {
            lightingUp.SetActive(true);
        }

        // Check to see if a wall is below
        if (tileDown != null && tileDown.tag == "Floor") {
            lightingDown.SetActive(true);
            isDownFacing = true;
        }

        // Check to see if a wall is left
        if (tileLeft != null && tileLeft.tag == "Floor") {
            lightingLeft.SetActive(true);
        }

        // Check to see if a wall is right
        if (tileRight != null && tileRight.tag == "Floor") {
            lightingRight.SetActive(true);
        }
    }

    // Assign tile sprites based on surrounding tiles
    void AssignTile() {
        if (rend == null) {
            return;
        }

        // Get a tile visual 
        if (isDownFacing && downFacingTiles.Length > 0) {
            tile = downFacingTiles[Random.Range(0, downFacingTiles.Length)];
        } else if (neutralTiles.Length > 0) {
            tile = neutralTiles[Random.Range(0, neutralTiles.Length)];
        }

        if (tile != null) {
            // Setup tile sprites
            rend.sprite = tile.mainSprite;

            rend.sharedMaterial.mainTexture = tile.mainSprite.texture;

            if (acceptsLight) {
                lightingUp.GetComponent<SpriteRenderer>().sprite    = tile.lightSourceUp;
                lightingDown.GetComponent<SpriteRenderer>().sprite  = tile.lightSourceDown;
                lightingLeft.GetComponent<SpriteRenderer>().sprite  = tile.lightSourceLeft;
                lightingRight.GetComponent<SpriteRenderer>().sprite = tile.lightSourceRight;
            }
        } else {
            Debug.Log("Error rendering tile, no tile assigned.");
        }
    }
}
