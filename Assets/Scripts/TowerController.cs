using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteGlow;

[RequireComponent(typeof(Rigidbody2D))]
public class TowerController : MonoBehaviour
{
    public float fireRate = 1f;
    public float damage = 1f;
    public float range = 10.0f;
    public enum towerType {
        Hunger,
        Sleep,
        Stamina,
        Thirst
    }

    public enum STATE {
        Idle
    }

    private Rigidbody2D rb;
    private Animator animator;

    public towerType type;
    public STATE state;

    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        state = STATE.Idle;
    }

    private void FixedUpdate() {
        switch (state) {
            case STATE.Idle:
                HandleIdle();
                break;
        }
    }

    private void HandleIdle() {
        animator.SetBool("isIdle", true);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
