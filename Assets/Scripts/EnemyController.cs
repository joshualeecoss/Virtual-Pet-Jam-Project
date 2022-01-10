using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    public enum STATE {
        Idle
    }

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
        Debug.Log(animator);
        animator.SetBool("isIdle", true);
    }
}
