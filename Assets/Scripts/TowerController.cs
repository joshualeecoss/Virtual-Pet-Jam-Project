using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteGlow;
using CodeMonkey;
using CodeMonkey.Utils;

[RequireComponent(typeof(Rigidbody2D))]
public class TowerController : MonoBehaviour
{
   public enum towerType {
        Hunger,
        Sleep,
        Stamina,
        Thirst
    }

    private Vector3 projectileShootFromPosition;
    public float range;
    private int damageAmount;
    private float shootTimerMax;
    private float shootTimer;
    private Animator animator;
    public towerType type;
    private SpriteGlowEffect glowEffect;

    private void Awake() {
        projectileShootFromPosition = transform.Find("ProjectileShootFromPosition").position;
        range = 60f;
        damageAmount = 25;
        shootTimerMax = .4f;
        animator = GetComponent<Animator>();
        glowEffect = GetComponent<SpriteGlowEffect>();

        if (type == towerType.Hunger) {
            var tower = GameObject.Find("HungerIcon");
            ChangeGlow(tower, transform.gameObject);
        } else if (type == towerType.Sleep) {
            var tower = GameObject.Find("SleepIcon");
            ChangeGlow(tower, transform.gameObject);
        } else if (type == towerType.Stamina) {
            var tower = GameObject.Find("StaminaIcon");
            ChangeGlow(tower, transform.gameObject);
        } else if (type == towerType.Thirst) {
            var tower = GameObject.Find("ThirstIcon");
            ChangeGlow(tower, transform.gameObject);
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            //CMDebug.TextPopupMouse("Click!");
            //Bullet.Create(projectileShootFromPosition, UtilsClass.GetMouseWorldPosition());
        }

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0) {
            shootTimer = shootTimerMax;
            Enemy enemy = GetClosestEnemy();
            if (enemy != null) {
                // Enemy in range!
                Bullet.Create(projectileShootFromPosition, enemy, damageAmount);
            }        
        }
        
    }

    private Enemy GetClosestEnemy() {
        return Enemy.GetClosestEnemy(transform.position, range);
    }

    public float GetRange() {
        return range;
    }

    private void ChangeGlow(GameObject tower, GameObject sprite) {
        glowEffect.GlowBrightness = 2.5f;
        glowEffect.GlowColor = tower.GetComponent<SpriteGlowEffect>().GlowColor;
    }

    public void UpgradeRange() {
        range += 10;
    }

    public void UpgradeDamageAmount() {
        damageAmount += 5;
    }

    private void OnMouseEnter() {
        UpgradeOverlay.Show_Static(this);
    }

}
