using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPetController : MonoBehaviour
{
    public enum STATE {
        Idle
    }

    public STATE state;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
}
