using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode {
    private Grid<GridNode> grid;
    public int x;
    public int y;
    
    private Texture2D texture;
    private Sprite mySprite;
    private SpriteRenderer sr;
    
    public int gCost;
    public int hCost;
    public int fCost;

    public bool isEmpty;
    public GridNode cameFromNode;

    public GridNode(Grid<GridNode> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isEmpty = true;

        Vector3 worldPos00 = grid.GetWorldPosition(x, y);
        Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
        Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
        Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);

        // Debug.DrawLine(worldPos00, worldPos01, Color.white, 999f);
        // Debug.DrawLine(worldPos00, worldPos10, Color.white, 999f);
        // Debug.DrawLine(worldPos01, worldPos11, Color.white, 999f);
        // Debug.DrawLine(worldPos10, worldPos11, Color.white, 999f);

        GameObject gameObject = new GameObject("Square");
        gameObject.AddComponent<SpriteRenderer>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        texture = new Texture2D(22, 22);
        mySprite = Sprite.Create(texture, new Rect(0,0,1,1), new Vector2(0.5f, 0.5f));
        gameObject.transform.position = worldPos00 + new Vector3(24, 24) * 0.5f;
        gameObject.transform.localScale = new Vector3(2000f, 2000f, 1f);
        if (isEmpty) {
            sr.color = new Color(1f, 1f, 1f, 0.5f);
        } else {
            sr.color = new Color(0f, 0f, 0f, 0f);
        }
        
        
        sr.sprite = mySprite;

    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public bool isItEmpty() {
        return isEmpty;
    }

    public void SetIsEmpty(bool isEmpty) {
        this.isEmpty = isEmpty;
        if (isEmpty == false) {
            sr.color = new Color(0f, 0f, 0f, 0f);
        }
    }
}
