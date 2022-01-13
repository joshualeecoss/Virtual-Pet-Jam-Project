using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteGlow;

public class ZoneMeterController : MonoBehaviour
{
    public const float INCREASE_RATE = 0.00005f;
    public const float DECAY_RATE = 0.00005f;
    private GameObject sprites;
    public GameObject statBar;

    public enum STATE {
        Increasing,
        Decreasing,
        Stopped
    }
    public STATE state;
    public float maxValue, currentValue;

    private void Start() {
        statBar = transform.Find("Bar/StatBar").gameObject;
        sprites = transform.Find("Sprites").gameObject;
        Glow(sprites);

        maxValue = statBar.transform.localScale.y;
        currentValue = 0.0f;
        statBar.transform.localScale = new Vector2(statBar.transform.localScale.x, currentValue);

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
            statBar.transform.localScale = new Vector2(statBar.transform.localScale.x, statBar.transform.localScale.y + DECAY_RATE);
        } else {
            state = STATE.Stopped;
        }
    }

    private void HandleDecreasing() {
        if (currentValue >= 0) {
            currentValue -= DECAY_RATE;
            statBar.transform.localScale = new Vector2(statBar.transform.localScale.x, statBar.transform.localScale.y - DECAY_RATE);
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
}
