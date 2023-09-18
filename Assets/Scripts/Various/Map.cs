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

    // Set tile locations for each child gameobject
    void SetTiles() {
        if (tiles.Count > 0 || tileParent == null) {
            return;
        }

        // Currently commented out as updates are being made to this system
        // foreach (Transform child in tileParent) {
            // tiles.Add(new TileLocation(child.gameObject, (int)child.position.x, (int)child.position.y));
        // }
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
}