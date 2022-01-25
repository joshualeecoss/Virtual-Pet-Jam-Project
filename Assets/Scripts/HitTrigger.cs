using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour {
    private bool hit = false;
    private Enemy enemy;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            hit = true;
            enemy = other.GetComponent<Enemy>();
        }
    }

    public bool GetHit()
    {
        return hit;
    }

    public Enemy GetEnemy()
    {
        return enemy;
    }

    public void SetHit(bool hit)
    {
        this.hit = hit;
    }
}
