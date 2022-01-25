using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPetController : MonoBehaviour
{
    private bool lightUp = false;
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
        // switch (state) {
        //     case STATE.Idle:
        //         HandleIdle();
        //         break;
        // }
    }

    private void HandleIdle() {
        //animator.SetBool("isIdle", true);
    }

    public void Hit() {
        StartCoroutine(HitAnimation());
    }

    public IEnumerator HitAnimation() {
        animator.SetTrigger("isHit");
        yield return new WaitForSeconds(0.05f);
        animator.ResetTrigger("isHit");
    }

    public bool LightUpSign() {
        return lightUp;
    }

    public void SetLightUpSign() {
        lightUp = !lightUp;
    }

}
