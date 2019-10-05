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
    
    List<TileByLocation> tiles = new List<TileByLocation>();

    // Start is called before the first frame update
    void Start()
    {
        // Instance setup
        if (BuildMap.instance == null) {
            BuildMap.instance = this;
        }

        if (map != null && colorMappings.Length > 0) {
            // Setup mapBuilt event
            if (mapBuilt == null) {
                mapBuilt = new UnityEvent();
            }

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
                    transform
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

    public GameObject GetTileByLocation(float x, float y) {
        foreach (TileByLocation tile in tiles) {
            if (tile.x == x && tile.y == y) {
                return tile.obj;
            }
        }

        return null;
    }
}

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
