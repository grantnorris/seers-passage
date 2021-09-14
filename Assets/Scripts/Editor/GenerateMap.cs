using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class GenerateMap : Editor
{   
    Map map;
    Sprite mapSprite;
    Texture2D mapTex;
    Transform tileParent;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        map = (Map)target;
        mapSprite = map.mapSprite;
        mapTex = map.mapTex;
        tileParent = map.tileParent;

        if (GUILayout.Button("Build Map")) {
            Build();
        }

        if (GUILayout.Button("Remove Map")) {
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
        map.SetInstance();

        if (map != null && mappings.Length > 0) {
            mapTex = texFromSprite();

            // Set up parent for instantiated tiles
            if (tileParent == null) {
                tileParent = new GameObject("Tiles").transform;
                tileParent.SetParent(map.transform);
                map.tileParent = tileParent;
            }

            // Loop through map pixels and place tiles
            for (int y = 0; y < mapTex.height; y++) {
                for (int x = 0; x < mapTex.width; x++) {
                    PlaceTile(x, y, mappings);
                }
            }
        }
    }

    // Removes data related to a previously generated map
    public void Remove() {
        foreach (Transform child in map.transform) {
            if (child.name == "Tiles")     {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    // Create a tile object at a given set of coordinates
    void PlaceTile(int x, int y, ColorToTile[] mappings) {
        Color pixelColor = mapTex.GetPixel(x, y);

        // Pixel is transparent, so stop
        if (pixelColor.a == 0) {
            return;
        }

        ColorToTile colorMapping = ColorMapping(pixelColor);

        if (colorMapping == null) {
            return;
        }

        GameObject curTile = TileAtPosition(new Vector3(x, y, 0));

        if (curTile != null && PrefabUtility.GetCorrespondingObjectFromSource(curTile) == colorMapping.prefab) {
            Debug.Log("curtile found, And prefab is the same! " + colorMapping.prefab.name);
            return;
        }


        Destroy(curTile);
        GameObject tile = PrefabUtility.InstantiatePrefab(colorMapping.prefab) as GameObject;
        tile.transform.position = new Vector3(x, y, 0);
        tile.transform.rotation = Quaternion.identity;
        tile.transform.SetParent(tileParent);
        tile.name = x + ", " + y + " - " + colorMapping.name;
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

    GameObject TileAtPosition(Vector3 pos) {
        foreach (Transform child in tileParent) {
            if (child.position != pos) {
                continue;
            }

            return child.gameObject;
        }

        return null;
    }
}
