using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Enemy : MonoBehaviour
{
    public interface IEnemyTargetable {
        Vector3 GetPosition();
        void Damage(Enemy attacker);
    }

    public static List<Enemy> enemyList = new List<Enemy>();

    public static Enemy GetClosestEnemy(Vector3 position, float maxRange) {
        Enemy closest = null;
        foreach (Enemy enemy in enemyList) {
            if (enemy.IsDead()) continue;
            if (Vector3.Distance(position, enemy.GetPosition()) <= maxRange) {
                if (closest == null) {
                    closest = enemy;
                } else {
                    if (Vector3.Distance(position, enemy.GetPosition()) < Vector3.Distance(position, closest.GetPosition())) {
                        closest = enemy;
                    }
                }
            }
        }
        return closest;
    }

    public static Enemy Create(Vector3 position) {
        Transform enemyTransform = Instantiate(GameAssets.i.enemyPrefab, position, Quaternion.identity);
        Enemy enemyHandler = enemyTransform.GetComponent<Enemy>();
        return enemyHandler;
    }

    private const float SPEED = 30f;
    private State state;
    private Vector3 lastMoveDir;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private float pathfindingTimer;
    private Func<IEnemyTargetable> getEnemyTarget;
    private HealthBar healthBar;
    public HealthSystem healthSystem;

    private enum State {
        Normal,
        Attacking,
        Busy
    }

    private void Awake() {
        enemyList.Add(this);
        SetStateNormal();

        healthSystem = new HealthSystem(100);
    }

    private void Start() {
        Transform healthBarTransform = Instantiate(GameAssets.i.healthBarPrefab, new Vector3(transform.position.x, transform.position.y + 10f), Quaternion.identity);
        healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.transform.parent = transform;

        healthBar.Setup(healthSystem);
    }

    private void SetGetTarget(Func<IEnemyTargetable> getEnemyTarget) {
        this.getEnemyTarget = getEnemyTarget;
    }

    private void Update() {
        pathfindingTimer -= Time.deltaTime;

        switch(state) {
            case State.Normal:
                HandleMovement();
                //FindTarget();
                break;
            case State.Attacking:
                break;
            case State.Busy:
                break;
        }
    }


    public bool IsDead() {
        return healthSystem.IsDead();
    }

    private void SetStateNormal() {
        state = State.Normal;
    }

    private void SetStateAttacking() {
        state = State.Attacking;
    }

    public void Damage(int damageAmount) {
        healthSystem.Damage(damageAmount);
        if (IsDead()) {
            Destroy(gameObject);
        }
    }

    public void Damage(IEnemyTargetable attacker) {
        healthSystem.Damage(30);
        if (IsDead()) {
            Destroy(gameObject);
        }
    }

    private void HandleMovement() {
        if (pathVectorList != null) {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f) {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * SPEED * Time.deltaTime;
            } else {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count) {
                    StopMoving();
                }
            }
        } 
    }

    private void StopMoving() {
        pathVectorList = null;
    }

    public void SetTargetPosition(Vector3 targetPosition) {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
        if (pathVectorList != null && pathVectorList.Count > 1) {
            pathVectorList.RemoveAt(0);
        }
    }

    public void SetPathVectorList(List<Vector3> pathVectorList) {
        this.pathVectorList = pathVectorList;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }


    
}
