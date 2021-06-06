using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class BuildMap : MonoBehaviour
{
    public static BuildMap instance;

    public UnityEvent mapBuilt;
    public Texture2D map;
    public ColorToTile[] colorMappings;
    public List<TileByLocation> tiles = new List<TileByLocation>();

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

    // Creates tile objects for a given map image
    public void Build() {
        // Remove any existing tiles
        Remove();

        if (map != null && colorMappings.Length > 0) {
            // Set up parent for instantiated tiles
            tileParent = new GameObject("Tiles").transform;
            tileParent.SetParent(transform);

            // Loop through map pixels and place tiles
            for (int y = 0; y < map.height; y++) {
                for (int x = 0; x < map.width; x++) {
                    PlaceTile(x, y);
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
    void PlaceTile(int x, int y) {
        Color pixelColor = map.GetPixel(x, y);

        // Pixel is transparent, so stop
        if (pixelColor.a == 0) {
            return;
        }

        // Loop through color mappings and create the relevant tile
        foreach (ColorToTile colorMapping in colorMappings) {
            if (pixelColor == colorMapping.color && colorMapping.prefab) {
                // Instantiate tile
                GameObject tile = Instantiate(
                    colorMapping.prefab,
                    new Vector3(x, y, 0),
                    Quaternion.identity,
                    tileParent
                );
                
                // Name tile
                tile.name = x + ", " + y;

                // Subscribe to mapBuilt event
                if (tile.GetComponent<TileDisplay>() != null) {
                    BuildMap.instance.mapBuilt.AddListener(tile.GetComponent<TileDisplay>().Initialise);
                } else if (tile.GetComponent<SurroundingTile>() != null) {
                    BuildMap.instance.mapBuilt.AddListener(tile.GetComponent<SurroundingTile>().Initialise);
                }

                tiles.Add(new TileByLocation(tile, x, y));
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
