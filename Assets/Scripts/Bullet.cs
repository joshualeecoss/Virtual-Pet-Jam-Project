using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils; 

public class Bullet : MonoBehaviour
{
    private const float BULLET_SPEED = 60f;
    
    public static void Create(Vector3 spawnPosition, Enemy enemy, int damageAmount) {
        Transform bulletTransform = Instantiate(GameAssets.i.bulletPrefab, spawnPosition, Quaternion.identity);

        Bullet bullet = bulletTransform.GetComponent<Bullet>();
        bullet.Setup(enemy, damageAmount);
    }

    private Enemy enemy;
    private int damageAmount;

    private void Setup(Enemy enemy, int damageAmount) {
        this.enemy = enemy;
        this.damageAmount = damageAmount;
    }

    private void Update() {
        if (enemy == null || enemy.IsDead()) {
            Destroy(gameObject);
            return;
        }
        Vector3 targetPosition = enemy.GetPosition();
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        transform.position += moveDir * BULLET_SPEED * Time.deltaTime;
        float angle = UtilsClass.GetAngleFromVectorFloat(moveDir);
        transform.eulerAngles = new Vector3(0, 0, angle);

        float destroySelfDistance = 1f;
        if (Vector3.Distance(transform.position, targetPosition) < destroySelfDistance) {
            enemy.Damage(damageAmount);
            Destroy(gameObject);
        }
    }

}
