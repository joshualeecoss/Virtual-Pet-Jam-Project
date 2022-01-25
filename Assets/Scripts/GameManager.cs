using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private float cash;
    private const float PRICE_INCREASE = 1.0f;
    private const int GRID_WIDTH = 8;
    private const int GRID_HEIGHT = 11;
    private const float TOWER_PRICE = 10f;
    private const float ENEMY_INITIAL_HEALTH = 100f;
    private const int ENEMY_INITIAL_WAVE_SIZE = 6;
    private const float ENEMY_HEALTH_INCREASE = 30f;
    private bool loaded;
    private float gridCellSize = 24f;
    public bool isGameActive;

    private Grid<GridNode> grid;
    private HitTrigger hitCollider;

    private StatMeterController hungerMeter;
    private StatMeterController sleepMeter;
    private StatMeterController staminaMeter;
    private StatMeterController thirstMeter;

    private ZoneMeterController zoneMeter;
    private Transform tower;
    private VirtualPetController virtualPet;
    private List<Transform> towerList;
    private List<Enemy> enemyList;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    public float enemyHealth;
    private int waveSize;

    private TextMeshProUGUI cashText;
    private TextMeshProUGUI hungerPrice, sleepPrice, staminaPrice, thirstPrice;
    private TextMeshProUGUI foodPrice, pillowPrice, coffeePrice, waterPrice;
    private float hungerPriceValue = 10f, sleepPriceValue = 10f, staminaPriceValue = 10f, thirstPriceValue = 10f;
    private float foodPriceValue = 1f, pillowPriceValue = 1f, coffeePriceValue = 1f, waterPriceValue = 1f;
    private AudioController sound;

    private void Awake() {
        grid = new Grid<GridNode>(8, 11, gridCellSize, new Vector3(262, 24), (Grid<GridNode> g, int x, int y) => new GridNode(g, x, y));
        towerList = new List<Transform>();
        enemyList = new List<Enemy>();
        hitCollider = GameObject.Find("HitTrigger").GetComponent<HitTrigger>();
        virtualPet = GameObject.Find("VirtualPet").GetComponent<VirtualPetController>();
        sound = GameObject.Find("AudioPlayer").GetComponent<AudioController>();
    }

    private void Start() {
        // sound.BGMusic();
        isGameActive = true;
        Time.timeScale = 1;
        enemyHealth = ENEMY_INITIAL_HEALTH;
        waveSize = ENEMY_INITIAL_WAVE_SIZE;
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

        loaded = false;
        cash = 50f;
        SetText();
        BlackOutPath();
        StartCoroutine(WaveTimeout());
    }

    private void Update() {
        SetText();
        ZoneCheck();
        ZonePower();
        EmptyMeterCheck();

        foreach (Enemy enemy in enemyList.ToArray()) {
            if (enemy.IsDead()) {
                sound.Die();
                enemyList.Remove(enemy);
                cash += UnityEngine.Random.Range(1, 3);
                if (enemyList.Count == 0) {
                    StartCoroutine(WaveTimeout());
                }
            }
        }

        if (hitCollider.GetHit()) {
            virtualPet.Hit();
            sound.Hit();
            if (hitCollider.GetEnemy() != null) {
                enemyList.Remove(hitCollider.GetEnemy());
                hitCollider.GetEnemy().ReachedTarget();
                zoneMeter.Damage();
            }
            hitCollider.SetHit(false);
            if (enemyList.Count == 0) {
                StartCoroutine(WaveTimeout());
            }
        }


        if (Input.GetMouseButtonDown(0)) {
            GridNode node = grid.GetGridObject(UtilsClass.GetMouseWorldPosition());
            if (node != null && loaded == true) {
                if (node.isItEmpty() == false) {
                    return;
                } else {
                    sound.SetTower();
                    SpawnTower();
                    node.SetIsEmpty(false);
                    loaded = false;
                }
            } 
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGameActive) {
                PauseGame();
            } else {
                ResumeGame();
            }
        }


        if (zoneMeter.GetCurrentValue() <= zoneMeter.GetGameOverValue()) {
            GameOver();
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

    private void ZonePower() {
        if (zoneMeter.GetCurrentValue() >= zoneMeter.GetThresholdValue()) {
            foreach (Transform tower in towerList) {
                tower.GetComponent<TowerController>().ZoneDamage();
                tower.GetComponent<Animator>().speed = 1.5f;
            }
        } else {
            foreach (Transform tower in towerList) {
                tower.GetComponent<TowerController>().RegularDamage();
                tower.GetComponent<Animator>().speed = 0.8f;
            }
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
        sound.Click();
        if (cash >= foodPriceValue) {
            cash -= foodPriceValue;
            foodPriceValue += PRICE_INCREASE;
            if (hungerMeter.GetCurrentValue() <= hungerMeter.GetMaxValue()) {
                hungerMeter.Fill();
            }
        }
    }

    public void PillowButtonClicked() {
        sound.Click();
        if (cash >= pillowPriceValue) {
            cash -= pillowPriceValue;
            pillowPriceValue += PRICE_INCREASE;
            if (sleepMeter.GetCurrentValue() <= sleepMeter.GetMaxValue()) {
                sleepMeter.Fill();
            }
        }
    }

    public void CoffeeButtonClicked() {
        sound.Click();
        if (cash >= coffeePriceValue) {
            cash -= coffeePriceValue;
            coffeePriceValue += PRICE_INCREASE;
            if (staminaMeter.GetCurrentValue() <= staminaMeter.GetMaxValue()) {
                staminaMeter.Fill();
            }
        }
    }

    public void WaterButtonClicked() {
        sound.Click();
        if (cash >= waterPriceValue) {
            cash -= waterPriceValue;
            waterPriceValue += PRICE_INCREASE;
            if (thirstMeter.GetCurrentValue() <= thirstMeter.GetMaxValue()) {
                thirstMeter.Fill();
            }
        }
    }

    public void HungerButtonClicked() {
        sound.Click();
        if (cash >= hungerPriceValue){
            ReadyTower(TowerController.towerType.Hunger);
        }
    }

    public void SleepButtonClicked() {
        sound.Click();
        if (cash >= sleepPriceValue) {
            ReadyTower(TowerController.towerType.Sleep);
        }

    }

    public void StaminaButtonClicked() {
        sound.Click();
        if (cash >= staminaPriceValue) {
            ReadyTower(TowerController.towerType.Stamina);
        }
    }

    public void ThirstButtonClicked() {
        sound.Click();
        if (cash >= thirstPriceValue) {
            ReadyTower(TowerController.towerType.Thirst);
        }
    }

    public void ReplayButtonClicked() {
        sound.Click();
        SceneManager.LoadScene("Main");
    }

    public void QuitButtonClicked() {
        sound.Click();
        Application.Quit();
    }


    private void ReadyTower(TowerController.towerType type) {
        tower = GameAssets.i.towerPrefab;
        tower.GetComponent<TowerController>().SetType(type);
        loaded = true;
    }

    private void SpawnTower() {
        if (tower != null) {
            if (tower.GetComponent<TowerController>().GetTowerType() == TowerController.towerType.Hunger) {
                hungerMeter.IncreaseDecayRate();
            } else if (tower.GetComponent<TowerController>().GetTowerType() == TowerController.towerType.Sleep) {
                sleepMeter.IncreaseDecayRate();
            } else if (tower.GetComponent<TowerController>().GetTowerType() == TowerController.towerType.Stamina) {
                staminaMeter.IncreaseDecayRate();
            } else if (tower.GetComponent<TowerController>().GetTowerType() == TowerController.towerType.Thirst) {
                thirstMeter.IncreaseDecayRate();
            } 

            Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
            spawnPosition = ValidateWorldGridPosition(spawnPosition);
            spawnPosition += new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;
            Transform newTower = Instantiate(tower, spawnPosition, Quaternion.identity);  
            towerList.Add(newTower);
            cash -= TOWER_PRICE; 
            tower = null;  
        }
    }

    private void EmptyMeterCheck() {
        if (hungerMeter.GetCurrentValue() <= 0) {
            foreach (Transform tower in towerList) {
                if (tower.GetComponent<TowerController>().GetTowerType() == TowerController.towerType.Hunger) {
                    tower.GetComponent<TowerController>().NoDamage();
                    tower.GetComponent<Animator>().speed = 0f;
                }
            }
        }
        if (sleepMeter.GetCurrentValue() <= 0) {
            foreach (Transform tower in towerList) {
                if (tower.GetComponent<TowerController>().GetTowerType() == TowerController.towerType.Sleep) {
                    tower.GetComponent<TowerController>().NoDamage();
                    tower.GetComponent<Animator>().speed = 0f;
                }
            }
        }
        if (staminaMeter.GetCurrentValue() <= 0) {
            foreach (Transform tower in towerList) {
                if (tower.GetComponent<TowerController>().GetTowerType() == TowerController.towerType.Stamina) {
                    tower.GetComponent<TowerController>().NoDamage();
                    tower.GetComponent<Animator>().speed = 0f;
                }
            }
        }
        if (thirstMeter.GetCurrentValue() <= 0) {
            foreach (Transform tower in towerList) {
                if (tower.GetComponent<TowerController>().GetTowerType() == TowerController.towerType.Thirst) {
                    tower.GetComponent<TowerController>().NoDamage();
                    tower.GetComponent<Animator>().speed = 0f;
                }
            }
        }
    }

    private Vector3 ValidateWorldGridPosition(Vector3 position) {
        grid.GetXY(position, out int x, out int y);
        return grid.GetWorldPosition(x, y);
    }

    private void SpawnEnemyWave() {
        float spawnTime = 0f;
        float timePerSpawn = .6f;

        for (int i = 0; i < waveSize; i++) {
            FunctionTimer.Create(() => SpawnEnemy(), spawnTime); spawnTime += timePerSpawn;
        }
        enemyHealth += 10;
        waveSize += UnityEngine.Random.Range(1, 3);
    }
    private void SpawnEnemy() {
        Vector3 spawnPosition = ValidateWorldGridPosition(new Vector3(465f, 252f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;
        List<Vector3> waypointPositionList = WaypointPositions();

        Enemy enemy = Enemy.Create(spawnPosition);
        enemy.SetPathVectorList(waypointPositionList);
        enemy.SetHealth(enemyHealth);
        enemyList.Add(enemy);

    }

    private List<Vector3> WaypointPositions() {
        List<Vector3> positionList = new List<Vector3>() { 
            ValidateWorldGridPosition(new Vector3(418f, 252f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(418f, 60f))  + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(298f, 60f))  + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(298f, 108f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(370f, 108f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(370f, 252f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(322f, 252f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(322f, 156f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(274f, 156f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(274f, 228f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(235f, 228f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f
        };

        return positionList;
    }

    private void BlackOutPath() {
        List<Vector3> nodes = new List<Vector3>() {
            ValidateWorldGridPosition(new Vector3(441f, 252f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(418f, 252f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(418f, 228f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(418f, 204f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(418f, 180f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(418f, 156f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(418f, 132f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(418f, 108f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(418f, 84f))  + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(418f, 60f))  + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(394f, 60f))  + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(370f, 60f))  + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(346f, 60f))  + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(322f, 60f))  + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(298f, 60f))  + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(298f, 84f))  + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(298f, 108f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(322f, 108f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(346f, 108f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(370f, 108f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(370f, 132f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(370f, 156f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(370f, 180f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(370f, 204f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(370f, 228f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(370f, 252f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(346f, 252f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(322f, 252f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(322f, 228f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(322f, 204f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(322f, 180f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(322f, 156f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(298f, 156f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(274f, 156f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(274f, 180f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(274f, 204f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            ValidateWorldGridPosition(new Vector3(274f, 228f)) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
        };

        foreach (Vector3 node in nodes) {
            grid.GetGridObject(node).SetIsEmpty(false);
        }

    }

    public void GameOver() {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void PauseGame() {
        isGameActive = false;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void ResumeGame() {
        isGameActive = true;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    private IEnumerator WaveTimeout() {
        yield return new WaitForSeconds(5f);
        SpawnEnemyWave();
    }


}
