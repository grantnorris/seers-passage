using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    public static Map instance;

    public Sprite mapSprite;
    [HideInInspector]
    public Texture2D mapTex;
    public Transform tileParent;
    // [HideInInspector]
    public List<TileLocation> tiles = new List<TileLocation>();
    [Tooltip("This value is used for nothing, this is simply a checkbox to trigger the prefab save.")]
    [SerializeField]
    bool save = false;

    void Awake() {
        if (tileParent == null && transform.childCount > 0) {
            tileParent = transform.GetChild(0);
        }

        SetInstance();
        SetTiles();
    }

    public void SetInstance() {
        if (instance == null) {
            instance = this;
        }
    }

    void SetTiles() {
        if (tiles.Count > 0 || tileParent == null) {
            return;
        }

        foreach (Transform child in tileParent) {
            // tiles.Add(new TileLocation(child.gameObject, (int)child.position.x, (int)child.position.y));
        }
    }

    // Get the tile object residing at the given coordinates
    public TileLocation GetTile(float x, float y) {
        foreach (TileLocation tile in tiles) {
            if (tile.x == x && tile.y == y) {
                return tile;
            }
        }

        return null;
    }

    ColorToTile[] ColorMappings() {
        return new ColorToTile[] {
            new ColorToTile(
                "Floor",
                new Color(1, 1, 1, 1),
                Resources.Load<GameObject>("Prefabs/Floor")
            ),
            new ColorToTile(
                "Wall",
                new Color(0, 0, 0, 1),
                Resources.Load<GameObject>("Prefabs/Wall")
            ),
            new ColorToTile(
                "Player Spawn",
                new Color(0.4352941f, 0.7450981f, 0.2666667f, 1),
                Resources.Load<GameObject>("Prefabs/Player Spawn")
            ),
            new ColorToTile(
                "Surround",
                new Color(0.9294118f, 0.1411765f, 0.5647059f, 1),
                Resources.Load<GameObject>("Prefabs/Surround")
            ),
            new ColorToTile(
                "Exit",
                new Color(0.7450981f, 0.2666667f, 0.2666667f, 1),
                Resources.Load<GameObject>("Prefabs/Exit")
            ),
            new ColorToTile(
                "Light Spawn",
                new Color(0f, 0.8235294f, 1f, 1),
                Resources.Load<GameObject>("Prefabs/Light Spawn")
            ),
            new ColorToTile(
                "Switch",
                new Color(1f, 0.8470588f, 0f, 1),
                Resources.Load<GameObject>("Prefabs/Switch")
            ),
            new ColorToTile(
                "Gate",
                new Color(1f, 0.6352941f, 0f, 1),
                Resources.Load<GameObject>("Prefabs/Gate")
            ),
            new ColorToTile(
                "Entrance",
                new Color(0.3529412f, 1f, 0f, 1),
                Resources.Load<GameObject>("Prefabs/Entrance")
            ),
            new ColorToTile(
                "Journal",
                new Color(0f, 0.4705882f, 1f, 1),
                Resources.Load<GameObject>("Prefabs/Journal")
            ),
            new ColorToTile(
                "Key Pickup",
                new Color(0.4941176f, 0f, 1f, 1),
                Resources.Load<GameObject>("Prefabs/Key Pickup")
            ),
            new ColorToTile(
                "Gate Locked",
                new Color(0.4039216f, 0.2901961f, 0.2f, 1),
                Resources.Load<GameObject>("Prefabs/Gate Locked")
            ),
            new ColorToTile(
                "Door",
                new Color(1, 0.3529412f, 0.3529412f, 1),
                Resources.Load<GameObject>("Prefabs/Door")
            )
        };
    }

    Texture2D texFromSprite() {
        Texture2D croppedTexture = new Texture2D((int)mapSprite.rect.width, (int)mapSprite.rect.height);
        Color[] pixels = mapSprite.texture.GetPixels(
            (int)mapSprite.textureRect.x, 
            (int)mapSprite.textureRect.y, 
            (int)mapSprite.textureRect.width, 
            (int)mapSprite.textureRect.height
        );
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }

    // Creates tile objects for a given map image
    public void Build() {
        ColorToTile[] mappings = ColorMappings();
        SetInstance();

        if (mappings.Length > 0) {
            mapTex = texFromSprite();

            // Set up parent for instantiated tiles
            if (tileParent == null) {
                tileParent = new GameObject("Tiles").transform;
                tileParent.SetParent(transform);
                tileParent = tileParent;
            }

            // Loop through map pixels and place tiles
            for (int y = 0; y < mapTex.height; y++) {
                for (int x = 0; x < mapTex.width; x++) {
                    PlaceTile(x, y);
                }
            }

            TreatTiles();
        }
    }

    // Removes data related to a previously generated map
    public void Remove() {
        foreach (Transform child in transform) {
            if (child.name == "Tiles")     {
                DestroyImmediate(child.gameObject);
            }
        }

        tiles = new List<TileLocation>();
    }

    // Create a tile object at a given set of coordinates
    void PlaceTile(int x, int y) {
        Color pixelColor = mapTex.GetPixel(x, y);

        // Pixel is transparent, so stop
        if (pixelColor.a == 0) {
            return;
        }

        ColorToTile colorMapping = ColorMapping(pixelColor);

        if (colorMapping == null) {
            return;
        }

        TileLocation curTile = GetTile(x, y);

        // Stop if tile at location exists and is the same type
        if (curTile != null && PrefabUtility.GetCorrespondingObjectFromSource(curTile.obj) == colorMapping.prefab) {
            return;
        }

        if (curTile != null) {
            Debug.Log("replace " + curTile.obj.name + " with " + colorMapping.name);
            tiles.Remove(curTile);
            DestroyImmediate(curTile.obj);
        }
        
        GameObject tile = PrefabUtility.InstantiatePrefab(colorMapping.prefab) as GameObject;
        tile.transform.position = new Vector3(x, y, 0);
        tile.transform.rotation = Quaternion.identity;
        tile.transform.SetParent(tileParent);
        tile.name = x + ", " + y + " - " + colorMapping.name;

        tiles.Add(new TileLocation(
            tile,
            x,
            y,
            colorMapping.name
        ));
    }

    void TreatTiles() {
        foreach (TileLocation tile in tiles) {
            Vector2 position = tile.obj.transform.position;

            switch (tile.type) {
                case "Player Spawn":
                    // Check tiles at surrounding locations to determine initial player direction

                    if (GetTile(position.x, position.y + 1).type == "Entrance") {
                        // Up
                        tile.obj.GetComponent<PlayerSpawn>().initialDirection = "down";
                    } else if (GetTile(position.x, position.y - 1).type == "Entrance") {
                        // Down
                        tile.obj.GetComponent<PlayerSpawn>().initialDirection = "up";
                    } else if (GetTile(position.x - 1, position.y).type == "Entrance") {
                        // Left
                        tile.obj.GetComponent<PlayerSpawn>().initialDirection = "right";
                    } else if (GetTile(position.x + 1, position.y).type == "Entrance") {
                        // Right
                        tile.obj.GetComponent<PlayerSpawn>().initialDirection = "right";
                    }
                    
                    break;

                case "Door":
                    // Check tiles at surrounding locations to determine initial player direction
                    if (GetTile(position.x, position.y + 1).type == "Floor") {
                        // Up
                        tile.obj.GetComponent<DoorInteractable>().direction = "up";
                        tile.obj.GetComponent<ExitVisual>().direction = "up";
                    } else if (GetTile(position.x, position.y - 1).type == "Floor") {
                        // Down
                        tile.obj.GetComponent<DoorInteractable>().direction = "down";
                        tile.obj.GetComponent<ExitVisual>().direction = "down";
                    } else if (GetTile(position.x - 1, position.y).type == "Floor") {
                        // Left
                        tile.obj.GetComponent<DoorInteractable>().direction = "left";
                        tile.obj.GetComponent<ExitVisual>().direction = "left";
                    } else if (GetTile(position.x + 1, position.y).type == "Floor") {
                        // Right
                        tile.obj.GetComponent<DoorInteractable>().direction = "right";
                        tile.obj.GetComponent<ExitVisual>().direction = "right";
                    }

                    break;
            }
        }
    }

    ColorToTile ColorMapping(Color color) {
        if (color == null) {
            return null;
        }

        foreach (ColorToTile colorMapping in ColorMappings()) {
            if (color != colorMapping.color) {
                continue;
            }
            
            return colorMapping;
        }

        return null;
    }
}

[System.Serializable]
public class TileLocation {
    public GameObject obj;
    public int x;
    public int y;
    public string type = "";

    public TileLocation(GameObject newObj, int newX, int newY, string newType) {
        obj  = newObj;
        x    = newX;
        y    = newY;
        type = newType;
    }
}