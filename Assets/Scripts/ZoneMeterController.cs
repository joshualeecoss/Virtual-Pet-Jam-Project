using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteGlow;

public class ZoneMeterController : MonoBehaviour
{
    private const float INCREASE_RATE = 0.00005f;
    private const float DECAY_RATE = 0.00005f;
    private GameObject sprites;
    public GameObject statBar;

    public enum STATE {
        Increasing,
        Decreasing,
        Stopped
    }
    public STATE state;
    public float maxValue, currentValue, thresholdValue, damageValue, gameOverValue;

    private void Start() {
        statBar = transform.Find("Bar/StatBar").gameObject;
        sprites = transform.Find("Sprites").gameObject;
        Glow(sprites);

        maxValue = statBar.transform.localScale.y;
        currentValue = 0.05f;
        thresholdValue = maxValue * 0.75f;
        statBar.transform.localScale = new Vector2(statBar.transform.localScale.x, currentValue);
        damageValue = maxValue * 0.05f;
        gameOverValue = maxValue * 0f;

        state = STATE.Increasing;
    }

    private void Update() {
        switch (state) {
            case STATE.Increasing:
                HandleIncreasing();
                break;
            case STATE.Decreasing:
                HandleDecreasing();
                break;
            case STATE.Stopped:
                HandleStopped();
                break;
        }
    }

    private void Glow(GameObject sprites) {
        for (int i = 0; i < sprites.transform.childCount; i++) {
            var sprite = sprites.transform.GetChild(i).gameObject;
            sprite.AddComponent<SpriteGlowEffect>();
            sprite.GetComponent<SpriteGlowEffect>().GlowBrightness = 2.5f;
            sprite.GetComponent<SpriteGlowEffect>().GlowColor = Color.red;
        }
    }

    private void HandleIncreasing() {
        if (currentValue <= maxValue) {
            currentValue += INCREASE_RATE;
            statBar.transform.localScale = new Vector2(statBar.transform.localScale.x, currentValue);
        } else {
            state = STATE.Stopped;
        }
    }

    private void HandleDecreasing() {
        if (currentValue >= 0) {
            currentValue -= DECAY_RATE;
            statBar.transform.localScale = new Vector2(statBar.transform.localScale.x, currentValue);
        } else {
            state = STATE.Stopped;
        }
    }

    private void HandleStopped() {
        return;
    }

    public void Increase() {
        state = STATE.Increasing;
    }

    public void Decrease() {
        state = STATE.Decreasing;
    }

    public float GetCurrentValue() {
        return currentValue;
    }

    public float GetThresholdValue() {
        return thresholdValue;
    }

    public float GetGameOverValue() {
        return gameOverValue;
    }

    public void Damage() {
        currentValue -= damageValue;
        statBar.transform.localScale = new Vector2(statBar.transform.localScale.x, currentValue);
    }
}
