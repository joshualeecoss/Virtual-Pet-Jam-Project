using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private Grid<bool> grid;
    private void Start() {
        grid = new Grid<bool>(8, 11, 24f, new Vector3(262, 24), (Grid<bool> grid, int x, int y) => true);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
        }
    }

    
}
