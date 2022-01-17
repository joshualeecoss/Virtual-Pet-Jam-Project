using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TowerDefenseAI : MonoBehaviour
{
    private Grid<GridNode> grid;

    private void Awake() {
        grid = new Grid<GridNode>(8, 11, 24f, new Vector3(262, 24), (Grid<GridNode> g, int x, int y) => new GridNode(g, x, y));
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.B)) {
            SpawnEnemyWave_1();
            Debug.Log("B");
        }

        if (Input.GetMouseButtonDown(1)) {
            SpawnTower();
        }
    }

    private void SpawnTower() {
        Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
        spawnPosition = ValidateWorldGridPosition(spawnPosition);
        spawnPosition += new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;

        Transform tower = GameAssets.i.towerPrefab;
        tower.GetComponent<TowerController>().type = TowerController.towerType.Stamina;

        Instantiate(tower, spawnPosition, Quaternion.identity);
        
    }

    private Vector3 ValidateWorldGridPosition(Vector3 position) {
        grid.GetXY(position, out int x, out int y);
        return grid.GetWorldPosition(x, y);
    }

    private void SpawnEnemyWave_1() {
        float spawnTime = 0f;
        float timePerSpawn = .6f;

        FunctionTimer.Create(() => SpawnEnemy(), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(), spawnTime); spawnTime += timePerSpawn;
    }
    private void SpawnEnemy() {
        Vector3 spawnPosition = new Vector3(461f, 178f);
        List<Vector3> waypointPositionList = new List<Vector3> {
            new Vector3(375f, 178f),
            new Vector3(375f, 120f),
            new Vector3(300f, 120f),
            new Vector3(300f, 178f),
        };

        Enemy enemy = Enemy.Create(spawnPosition);
        enemy.SetPathVectorList(waypointPositionList);
    }

    public class GridNode {
        private Grid<GridNode> grid;
        private int x;
        private int y;
        private bool empty;

        public GridNode(Grid<GridNode> grid, int x, int y, bool empty = true) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            this.empty = empty;

            Vector3 worldPos00 = grid.GetWorldPosition(x, y);
            Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
            Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
            Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);

            Debug.DrawLine(worldPos00, worldPos01, Color.white, 999f);
            Debug.DrawLine(worldPos00, worldPos10, Color.white, 999f);
            Debug.DrawLine(worldPos01, worldPos11, Color.white, 999f);
            Debug.DrawLine(worldPos10, worldPos11, Color.white, 999f);
        }
    }
}
