using UnityEngine;

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

    protected TileLocation tileUp;
    protected TileLocation tileDown;
    protected TileLocation tileLeft;
    protected TileLocation tileRight;

    SpriteRenderer rend;
    bool isDownFacing = false;

    void Start() {
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

    // Set references to the surrounding tiles in the up, down, left and right directions
    void SurroundingTiles() {
        Vector2 position  = transform.position;
        Map map = Map.instance;
        tileUp = map.GetTile(position.x, position.y + 1);
        tileDown = map.GetTile(position.x, position.y - 1);
        tileLeft = map.GetTile(position.x - 1, position.y);
        tileRight = map.GetTile(position.x + 1, position.y);
    }

    // Enable directional light sources based on the surrounding tiles
    void EnableLightSources() {
        if (!acceptsLight || gameObject.tag == "Floor") {
            return;
        }

        if (TileIsFloor(tileUp)) {
            lightingUp.SetActive(true);
        }

        if (TileIsFloor(tileDown)) {
            lightingDown.SetActive(true);
            isDownFacing = true;
        }

        if (TileIsFloor(tileLeft)) {
            lightingLeft.SetActive(true);
        }

        if (TileIsFloor(tileRight)) {
            lightingRight.SetActive(true);
        }
    }

    // Whether or not a given tile has the "floor" tag
    bool TileIsFloor(TileLocation tileToCheck) {
        if (tileToCheck == null) {
            return false;
        }

        return tileToCheck.obj.tag == "Floor";
    }

    // Assign tile sprites based on surrounding tiles
    void AssignTile() {
        if (rend == null) {
            return;
        }

        // Get a tile visual 
        if (isDownFacing && downFacingTiles.Length > 0) {
            // Down facing tiles look different to all others
            tile = downFacingTiles[Random.Range(0, downFacingTiles.Length)];
        } else if (neutralTiles.Length > 0) {
            tile = neutralTiles[Random.Range(0, neutralTiles.Length)];
        }

        if (tile != null) {
            // Setup tile sprites
            rend.sprite = tile.mainSprite;

            rend.sharedMaterial.mainTexture = tile.mainSprite.texture;

            if (acceptsLight) {
                // Set tile lighting gameobject sprites
                lightingUp.GetComponent<SpriteRenderer>().sprite = tile.lightSourceUp;
                lightingDown.GetComponent<SpriteRenderer>().sprite = tile.lightSourceDown;
                lightingLeft.GetComponent<SpriteRenderer>().sprite = tile.lightSourceLeft;
                lightingRight.GetComponent<SpriteRenderer>().sprite = tile.lightSourceRight;
            }
        } else {
            Logger.Send("Error rendering tile, no tile assigned.", "general", "assertion");
        }
    }
}
