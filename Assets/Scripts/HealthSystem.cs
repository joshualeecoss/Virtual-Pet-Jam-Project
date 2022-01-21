using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem {

    public event EventHandler OnHealthChanged;
    private float health;
    private float healthMax;
    public HealthSystem(float healthMax) {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public float GetHealth() {
        return health;
    }

    public bool IsDead() {
        return health <= 0;
    }

    public float GetHealthPercent() {
        return health / healthMax;
    }

    public void Damage(float damageAmount) {
        health -= damageAmount;
        if (health < 0) {
            health = 0;
        }
        if (OnHealthChanged != null) {
            OnHealthChanged(this, EventArgs.Empty);
        }
    }

    public void Heal(float healAmount) {
        health += healAmount;
        if (health > healthMax) health = healthMax;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
}
