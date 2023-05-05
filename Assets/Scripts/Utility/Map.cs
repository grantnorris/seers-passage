﻿using System.Collections;
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