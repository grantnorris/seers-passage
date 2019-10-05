using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    float distance = 3f;

    public List<Transform> colliding = new List<Transform>();
    public List<LitTile> litTiles    = new List<LitTile>();

    CircleCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0f, 0f, 2.5f);

        if (col != null) {
            col.radius = distance;
        }

        GenerateLight();
    }

    void Update()
    {
        GenerateLight();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!colliding.Contains(other.transform)) {
            colliding.Add(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (colliding.Contains(other.transform)) {
            colliding.Remove(other.transform);
            UnlightTile(new LitTile(other.transform, ""));
        }
    }

    void GenerateLight() {
        // Draw a bunch of raycasts from the light source to see what we hit
        List<RaycastHit2D> lightHits = new List<RaycastHit2D>();

        Vector3 lightSource = transform.position;

        for (int x = 0; x < (distance * 4) + 1; x++) {
            for (int y = 0; y < (distance * 4) + 1; y++) {
                Vector3 rayEnd = new Vector3(
                    -(distance) + (float)(x * 0.5f) + .1f,
                    -(distance) + (float)(y * 0.5f)
                );

                Vector3 rayDirection = lightSource - rayEnd;
                
                Debug.DrawRay(lightSource, rayEnd, Color.magenta, .05f);

                RaycastHit2D hit = Physics2D.Linecast(lightSource, lightSource + rayEnd);

                if (hit && !lightHits.Contains(hit)) {
                    lightHits.Add(hit);
                }
            }
        }

        // Check our already lit tiles and check to see if theyre still colliding
        List<LitTile> tilesToRemove = new List<LitTile>();

        foreach (LitTile litTile in litTiles) {
            if (!colliding.Contains(litTile.obj)) {
                tilesToRemove.Add(litTile);
            }
        }

        foreach (LitTile litTile in tilesToRemove) {
            litTiles.Remove(litTile);
        }

        // Check each transform that our collider has hit and see if our light hit it
        // Light won't have hit the transform if the light's ray was obscure by another object
        foreach (Transform collision in colliding) {
            bool collisionHasLight = false;
            string lightDirection  = "";

            foreach (RaycastHit2D hit in lightHits) {
                if (hit.transform == collision) {
                    collisionHasLight = true;
                    lightDirection    = GetLightDirection(hit, collision);
                    break;
                }
            }

            LitTile foundTile = GetLitTile(collision.transform, lightDirection, litTiles);

            if (collisionHasLight && foundTile == null) {
                LightTile(new LitTile(collision.transform, lightDirection));
            } else if (!collisionHasLight && foundTile != null) {
                UnlightTile(foundTile);
            }
        }
    }

    // Get the direction of the light source
    string GetLightDirection(RaycastHit2D hit, Transform collision) {
        Vector2 positionDif = hit.point - new Vector2(collision.position.x, collision.position.y);
        Vector2 positiveDif = positionDif;
        string direction = "";

        if (hit.point.x == hit.transform.position.x - .5) {
            direction = "left";
        } else if (hit.point.x == hit.transform.position.x + .5) {
            direction = "right";
        } else if (hit.point.y == hit.transform.position.y + .5) {
            direction = "up";
        } else if (hit.point.y == hit.transform.position.y - .5) {
            direction = "down";
        }
        
        return direction;
    }

    // Made a tile on the map light up
    void LightTile(LitTile litTile) {
        TileDisplay tileDisplay = litTile.obj.GetComponent<TileDisplay>();

        if (tileDisplay != null && litTile.direction != "" && !litTiles.Contains(litTile)) {
            if (litTile.direction == "up") {
                if (tileDisplay.lightingUp != null) {
                    tileDisplay.lightingUp.SetActive(true);
                }
            } else if (litTile.direction == "down") {
                if (tileDisplay.lightingDown != null) {
                    tileDisplay.lightingDown.SetActive(true);
                }
            } else if (litTile.direction == "left") {
                if (tileDisplay.lightingLeft != null) {
                    tileDisplay.lightingLeft.SetActive(true);
                }
            } else if (litTile.direction == "right") {
                if (tileDisplay.lightingRight != null) {
                    tileDisplay.lightingRight.SetActive(true);
                }
            }

            litTiles.Add(litTile);
        }
    }

    // Stop lighting up a tile on the map
    void UnlightTile(LitTile litTile) {
        litTiles.Remove(litTile);
        TileDisplay tileDisplay = litTile.obj.GetComponent<TileDisplay>();

        if (tileDisplay != null) {
            // Take off light by direction
            if (litTile.direction == "up") {
                tileDisplay.lightingUp.SetActive(false);
            } else if (litTile.direction == "down") {
                tileDisplay.lightingDown.SetActive(false);
            } else if (litTile.direction == "left") {
                tileDisplay.lightingLeft.SetActive(false);
            } else if (litTile.direction == "right") {
                tileDisplay.lightingRight.SetActive(false);
            } else {
                // Take all light off!
                tileDisplay.lightingUp.SetActive(false);
                tileDisplay.lightingDown.SetActive(false);
                tileDisplay.lightingLeft.SetActive(false);
                tileDisplay.lightingRight.SetActive(false);
            }
        }
    }

    LitTile GetLitTile(Transform obj, string direction, List<LitTile> list) {
        foreach (LitTile litTile in list) {
            if (obj == litTile.obj && direction == litTile.direction) {
                return litTile;
            }
        }

        return null;
    }
}

[System.Serializable]
public class LitTile {
    public Transform obj;
    public string direction;

    public LitTile(Transform newObj, string newDirection) {
        obj       = newObj;
        direction = newDirection;
    }
}
