using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class GameManager : MonoBehaviour
{
    private float cash;
    private const float PRICE_INCREASE = 1.0f;

    private Grid<GridNode> grid;

    private StatMeterController hungerMeter;
    private StatMeterController sleepMeter;
    private StatMeterController staminaMeter;
    private StatMeterController thirstMeter;

    private ZoneMeterController zoneMeter;
    private Transform tower;

    private TextMeshProUGUI cashText;
    private TextMeshProUGUI hungerPrice, sleepPrice, staminaPrice, thirstPrice;
    private TextMeshProUGUI foodPrice, pillowPrice, coffeePrice, waterPrice;
    private float hungerPriceValue = 10f, sleepPriceValue = 10f, staminaPriceValue = 10f, thirstPriceValue = 10f;
    private float foodPriceValue = 1f, pillowPriceValue = 1f, coffeePriceValue = 1f, waterPriceValue = 1f;

    private void Awake() {
        grid = new Grid<GridNode>(8, 11, 24f, new Vector3(262, 24), (Grid<GridNode> g, int x, int y) => new GridNode(g, x, y));
    }

    private void Start() {
        cashText = GameObject.Find("CASH").GetComponent<TextMeshProUGUI>();
        hungerPrice = GameObject.Find("HungerPrice").GetComponent<TextMeshProUGUI>();
        sleepPrice = GameObject.Find("SleepPrice").GetComponent<TextMeshProUGUI>();
        staminaPrice = GameObject.Find("StaminaPrice").GetComponent<TextMeshProUGUI>();
        thirstPrice = GameObject.Find("ThirstPrice").GetComponent<TextMeshProUGUI>();
        foodPrice = GameObject.Find("FoodPrice").GetComponent<TextMeshProUGUI>();
        pillowPrice = GameObject.Find("PillowPrice").GetComponent<TextMeshProUGUI>();
        coffeePrice = GameObject.Find("CoffeePrice").GetComponent<TextMeshProUGUI>();
        waterPrice = GameObject.Find("WaterPrice").GetComponent<TextMeshProUGUI>();

        hungerMeter = GameObject.Find("Meters/HungerMeter").GetComponent<StatMeterController>();
        sleepMeter = GameObject.Find("Meters/SleepMeter").GetComponent<StatMeterController>();
        staminaMeter = GameObject.Find("Meters/StaminaMeter").GetComponent<StatMeterController>();
        thirstMeter = GameObject.Find("Meters/ThirstMeter").GetComponent<StatMeterController>();
        zoneMeter = GameObject.Find("Meters/TheZoneMeter").GetComponent<ZoneMeterController>();

        
        cash = 500f;
        SetText();
    }

    private void Update() {
        SetText();
        ZoneCheck();

        if (Input.GetKeyDown(KeyCode.B)) {
            SpawnEnemyWave_1();
            Debug.Log("B");
        }

        if (Input.GetMouseButtonDown(1)) {
            SpawnTower();
        }
    }

    private void ZoneCheck() {
        if (hungerMeter.GetCurrentValue() >= hungerMeter.GetThreshold() &&
            sleepMeter.GetCurrentValue() >= sleepMeter.GetThreshold() &&
            staminaMeter.GetCurrentValue() >= staminaMeter.GetThreshold() &&
            thirstMeter.GetCurrentValue() >= thirstMeter.GetThreshold()) {
                zoneMeter.Increase();
            } else {
                zoneMeter.Decrease();
            }
    }

    private void SetText() {
        cashText.SetText("CASH:$" + cash);
        hungerPrice.SetText("$" + hungerPriceValue);    sleepPrice.SetText("$" + sleepPriceValue);
        staminaPrice.SetText("$" + staminaPriceValue);  thirstPrice.SetText("$" + thirstPriceValue);
        foodPrice.SetText("$" + foodPriceValue);        pillowPrice.SetText("$" + pillowPriceValue);
        coffeePrice.SetText("$" + coffeePriceValue);    waterPrice.SetText("$" + waterPriceValue);
    }

    public void FoodButtonClicked() {
        if (cash >= foodPriceValue) {
            cash -= foodPriceValue;
            foodPriceValue += PRICE_INCREASE;
            if (hungerMeter.GetCurrentValue() <= hungerMeter.GetMaxValue()) {
                hungerMeter.Fill();
            }
        }
    }

    public void PillowButtonClicked() {
        if (cash >= pillowPriceValue) {
            cash -= pillowPriceValue;
            pillowPriceValue += PRICE_INCREASE;
            if (sleepMeter.GetCurrentValue() <= sleepMeter.GetMaxValue()) {
                sleepMeter.Fill();
            }
        }
    }

    public void CoffeeButtonClicked() {
        if (cash >= coffeePriceValue) {
            cash -= coffeePriceValue;
            coffeePriceValue += PRICE_INCREASE;
            if (staminaMeter.GetCurrentValue() <= staminaMeter.GetMaxValue()) {
                staminaMeter.Fill();
            }
        }
    }

    public void WaterButtonClicked() {
        if (cash >= waterPriceValue) {
            cash -= waterPriceValue;
            waterPriceValue += PRICE_INCREASE;
            if (thirstMeter.GetCurrentValue() <= thirstMeter.GetMaxValue()) {
                thirstMeter.Fill();
            }
        }
    }

    public void HungerButtonClicked() {
        ReadyTower(TowerController.towerType.Hunger);
    }

    public void SleepButtonClicked() {
        ReadyTower(TowerController.towerType.Sleep);
    }

    public void StaminaButtonClicked() {
        ReadyTower(TowerController.towerType.Stamina);
    }

    public void ThirstButtonClicked() {
        ReadyTower(TowerController.towerType.Thirst);
    }

    private void ReadyTower(TowerController.towerType type) {
        tower = GameAssets.i.towerPrefab;
        tower.GetComponent<TowerController>().SetType(type);
    }

    private void SpawnTower() {
        if (tower != null) {
            Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
            spawnPosition = ValidateWorldGridPosition(spawnPosition);
            spawnPosition += new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;
            Instantiate(tower, spawnPosition, Quaternion.identity);   
            tower = null;  
        }
 
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

}
