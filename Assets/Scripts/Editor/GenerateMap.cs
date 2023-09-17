using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class GenerateMap : Editor
{   
    Map map;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Build Map")) {
            map = (Map)target;
            Build();
        }

        if (GUILayout.Button("Remove Map")) {
            map = (Map)target;
            Remove();
        }
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
        Texture2D croppedTexture = new Texture2D((int)map.mapSprite.rect.width, (int)map.mapSprite.rect.height);
        Color[] pixels = map.mapSprite.texture.GetPixels(
            (int)map.mapSprite.textureRect.x, 
            (int)map.mapSprite.textureRect.y, 
            (int)map.mapSprite.textureRect.width, 
            (int)map.mapSprite.textureRect.height
        );
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }

    // Creates tile objects for a given map image
    public void Build() {
        ColorToTile[] mappings = ColorMappings();
        map.SetInstance();

        if (mappings.Length > 0) {
            map.mapTex = texFromSprite();

            // Set up parent for instantiated map.tiles
            if (map.tileParent == null) {
                map.tileParent = new GameObject("Tiles").transform;
                map.tileParent.SetParent(map.transform);
            }

            // Loop through map pixels and place map.tiles
            for (int y = 0; y < map.mapTex.height; y++) {
                for (int x = 0; x < map.mapTex.width; x++) {
                    PlaceTile(x, y);
                }
            }

            TreatTiles();
        }
    }

    // Create a tile object at a given set of coordinates
    void PlaceTile(int x, int y) {
        Color pixelColor = map.mapTex.GetPixel(x, y);

        // Pixel is transparent, so stop
        if (pixelColor.a == 0) {
            return;
        }

        ColorToTile colorMapping = ColorMapping(pixelColor);

        if (colorMapping == null) {
            return;
        }

        TileLocation curTile = map.GetTile(x, y);

        // Stop if tile at location exists and is the same type
        if (curTile != null && curTile.type == colorMapping.name) {
            return;
        }

        if (curTile != null) {
            Debug.Log("Replace " + curTile.obj.name + " with " + colorMapping.name);
            map.tiles.Remove(curTile);
            DestroyImmediate(curTile.obj);
        }
        
        GameObject tile = PrefabUtility.InstantiatePrefab(colorMapping.prefab) as GameObject;
        tile.transform.position = new Vector3(x, y, 0);
        tile.transform.rotation = Quaternion.identity;
        tile.transform.SetParent(map.tileParent);
        tile.name = x + ", " + y + " - " + colorMapping.name;

        map.tiles.Add(new TileLocation(
            tile,
            x,
            y,
            colorMapping.name
        ));
    }

    void TreatTiles() {
        foreach (TileLocation tile in map.tiles) {
            Vector2 position = tile.obj.transform.position;

            switch (tile.type) {
                case "Player Spawn":
                    // Check map.tiles at surrounding locations to determine initial player direction

                    if (map.GetTile(position.x, position.y + 1).type == "Entrance") {
                        // Up
                        tile.obj.GetComponent<PlayerSpawn>().initialDirection = "down";
                    } else if (map.GetTile(position.x, position.y - 1).type == "Entrance") {
                        // Down
                        tile.obj.GetComponent<PlayerSpawn>().initialDirection = "up";
                    } else if (map.GetTile(position.x - 1, position.y).type == "Entrance") {
                        // Left
                        tile.obj.GetComponent<PlayerSpawn>().initialDirection = "right";
                    } else if (map.GetTile(position.x + 1, position.y).type == "Entrance") {
                        // Right
                        tile.obj.GetComponent<PlayerSpawn>().initialDirection = "right";
                    }
                    
                    break;

                case "Door":
                    // Check map.tiles at surrounding locations to determine initial player direction
                    if (map.GetTile(position.x, position.y + 1).type == "Floor") {
                        // Up
                        tile.obj.GetComponent<DoorInteractable>().direction = "up";
                        tile.obj.GetComponent<ExitVisual>().direction = "up";
                    } else if (map.GetTile(position.x, position.y - 1).type == "Floor") {
                        // Down
                        tile.obj.GetComponent<DoorInteractable>().direction = "down";
                        tile.obj.GetComponent<ExitVisual>().direction = "down";
                    } else if (map.GetTile(position.x - 1, position.y).type == "Floor") {
                        // Left
                        tile.obj.GetComponent<DoorInteractable>().direction = "left";
                        tile.obj.GetComponent<ExitVisual>().direction = "left";
                    } else if (map.GetTile(position.x + 1, position.y).type == "Floor") {
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

    // Removes data related to a previously generated map
    public void Remove() {
        foreach (Transform child in map.transform) {
            if (child.name == "Tiles")     {
                DestroyImmediate(child.gameObject);
            }
        }

        map.tiles = new List<TileLocation>();
    }
}
