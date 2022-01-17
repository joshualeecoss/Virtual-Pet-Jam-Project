using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode {
    private Grid<GridNode> grid;
    public int x;
    public int y;
    
    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public GridNode cameFromNode;

    public GridNode(Grid<GridNode> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;

        Vector3 worldPos00 = grid.GetWorldPosition(x, y);
        Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
        Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
        Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);

        Debug.DrawLine(worldPos00, worldPos01, Color.white, 999f);
        Debug.DrawLine(worldPos00, worldPos10, Color.white, 999f);
        Debug.DrawLine(worldPos01, worldPos11, Color.white, 999f);
        Debug.DrawLine(worldPos10, worldPos11, Color.white, 999f);
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }
}
