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
    [HideInInspector]
    public List<TileLocation> tiles = new List<TileLocation>();

    void Awake() {
        SetInstance();
    }

    public void SetInstance() {
        if (instance == null) {
            instance = this;
        }
    }

    // Get the tile object residing at the given coordinates
    public GameObject GetTile(float x, float y) {
        foreach (TileLocation tile in tiles) {
            if (tile.x == x && tile.y == y) {
                return tile.obj;
            }
        }

        return null;
    }
}

public class TileLocation {
    public GameObject obj;
    public int x;
    public int y;

    public TileLocation(GameObject newObj, int newX, int newY) {
        obj  = newObj;
        x    = newX;
        y    = newY;
    }
}