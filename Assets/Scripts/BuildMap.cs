﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class BuildMap : MonoBehaviour
{
    public static BuildMap instance;

    [HideInInspector]
    public UnityEvent mapBuilt;
    public Sprite map;
    public ColorToTile[] colorMappings;
    public List<TileByLocation> tiles = new List<TileByLocation>();
    Texture2D mapTex;

    Transform tileParent;

    void Awake() {
        // Instance setup
        if (instance == null) {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup mapBuilt event
        if (mapBuilt == null) {
            mapBuilt = new UnityEvent();
        }

        // If there is no parent set, make this the parent
        if (tileParent == null) {
            tileParent = transform;
        }
    }

    ColorToTile[] ColorMappings() {
        return colorMappings;
        // return new ColorToTile[] {
        //     new ColorToTile(
        //         "floor",
        //         new Color32(255, 255, 255, 1),
        //         Resources.Load<GameObject>("Prefabs/Floor")
        //     ),
        //     new ColorToTile(
        //         "wall",
        //         new Color32(0, 0, 0, 1),
        //         Resources.Load<GameObject>("Prefabs/Wall Tile")
        //     )
        // };
    }

    Texture2D texFromSprite() {
        Texture2D croppedTexture = new Texture2D( (int)map.rect.width, (int)map.rect.height );
        Color[] pixels = map.texture.GetPixels(
            (int)map.textureRect.x, 
            (int)map.textureRect.y, 
            (int)map.textureRect.width, 
            (int)map.textureRect.height
        );
        croppedTexture.SetPixels( pixels );
        croppedTexture.Apply();
        return croppedTexture;
    }

    // Creates tile objects for a given map image
    public void Build() {
        if (instance == null) {
            instance = this;
        }

        // Remove any existing tiles
        Remove();

        ColorToTile[] mappings = ColorMappings();

        if (map != null && mappings.Length > 0) {
            mapTex = texFromSprite();

            // Set up parent for instantiated tiles
            tileParent = new GameObject("Tiles").transform;
            tileParent.SetParent(transform);

            // Loop through map pixels and place tiles
            for (int y = 0; y < mapTex.height; y++) {
                for (int x = 0; x < mapTex.width; x++) {
                    PlaceTile(x, y, mappings);
                }
            }

            // Invoke mapBuilt event
            if (mapBuilt != null) {
                mapBuilt.Invoke();
            }
        }
    }

    // Removes data related to a previously generated map
    public void Remove() {
        foreach (Transform child in transform) {
            if (child.name == "Tiles")     {
                DestroyImmediate(child.gameObject);
            }
        }

        if (tiles.Count > 0) {
            tiles.Clear();
        }
    }

    // Create a tile object at a given set of coordinates
    void PlaceTile(int x, int y, ColorToTile[] mappings) {
        Color pixelColor = mapTex.GetPixel(x, y);

        // Pixel is transparent, so stop
        if (pixelColor.a == 0) {
            return;
        }

        // Loop through color mappings and create the relevant tile
        foreach (ColorToTile colorMapping in mappings) {
            if (pixelColor == colorMapping.color && colorMapping.prefab) {
                // Instantiate tile
                GameObject tile = PrefabUtility.InstantiatePrefab(colorMapping.prefab) as GameObject;
                tile.transform.position = new Vector3(x, y, 0);
                tile.transform.rotation = Quaternion.identity;
                tile.transform.SetParent(tileParent);
                
                // Name tile
                tile.name = x + ", " + y + " - " + colorMapping.name;

                // Subscribe to mapBuilt event
                if (tile.GetComponent<TileDisplay>() != null) {
                    BuildMap.instance.mapBuilt.AddListener(tile.GetComponent<TileDisplay>().Initialise);
                } else if (tile.GetComponent<SurroundingTile>() != null) {
                    BuildMap.instance.mapBuilt.AddListener(tile.GetComponent<SurroundingTile>().Initialise);
                }

                tiles.Add(new TileByLocation(tile, x, y));
                break;
            } else if (pixelColor == colorMapping.color && colorMapping.prefab == null) {
                Debug.Log("no prefab for tile");
            }
        }
    }
    
    // Get the tile object residing at the given coordinates
    public GameObject GetTileByLocation(float x, float y) {
        foreach (TileByLocation tile in tiles) {
            if (tile.x == x && tile.y == y) {
                return tile.obj;
            }
        }

        return null;
    }
}

[System.Serializable]
public class TileByLocation {
    public GameObject obj;
    public int x;
    public int y;

    public TileByLocation(GameObject newObj, int newX, int newY) {
        obj  = newObj;
        x    = newX;
        y    = newY;
    }
}
