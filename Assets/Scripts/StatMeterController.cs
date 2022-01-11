using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteGlow;

public class StatMeterController : MonoBehaviour
{
    public enum meterType {
        Hunger,
        Sleep,
        Stamina,
        Thirst
    }

    public enum STATE {
        Decreasing,
        Stopped
    }

    private STATE state;

    public const float DECAY_RATE = 0.0005f;

    public float maxValue, currentValue;
    public GameObject statBar;

    public meterType type;

    private GameObject sprites;

    private void Start() {
        sprites = transform.Find("Sprites").gameObject;
        statBar = transform.Find("Bar/StatBar").gameObject;
        
        if (type == meterType.Hunger) {
            var tower = GameObject.Find("HungerIcon");
            for (int i = 0; i < sprites.transform.childCount; i++) {
                var sprite = sprites.transform.GetChild(i).gameObject;
                ChangeGlow(tower, sprite);
            }
        } else if (type == meterType.Sleep) {
            var tower = GameObject.Find("SleepIcon");
            for (int i = 0; i < sprites.transform.childCount; i++) {
                var sprite = sprites.transform.GetChild(i).gameObject;
                ChangeGlow(tower, sprite);
            }
        } else if (type == meterType.Stamina) {
            var tower = GameObject.Find("StaminaIcon");
            for (int i = 0; i < sprites.transform.childCount; i++) {
                var sprite = sprites.transform.GetChild(i).gameObject;
                ChangeGlow(tower, sprite);
            }
        } else if (type == meterType.Thirst) {
            var tower = GameObject.Find("ThirstIcon");
            for (int i = 0; i < sprites.transform.childCount; i++) {
                var sprite = sprites.transform.GetChild(i).gameObject;
                ChangeGlow(tower, sprite);
            }
        }

        maxValue = statBar.transform.localScale.y;
        currentValue = maxValue;

        state = STATE.Decreasing;
    }

    private void Update() {
        switch (state) {
            case STATE.Decreasing:
                HandleDecreasing();
                break;
            case STATE.Stopped:
                HandleStopped();
                break;
        }  
    }

    private void HandleDecreasing() {
        if (currentValue > 0) {
            currentValue -= DECAY_RATE;
            statBar.transform.localScale = new Vector2(statBar.transform.localScale.x, currentValue);
        } else {
            state = STATE.Stopped;
        }
    }

    private void HandleStopped() {
        return;
    }

    private void ChangeGlow(GameObject tower, GameObject sprite) {
        sprite.AddComponent<SpriteGlowEffect>();
        sprite.GetComponent<SpriteGlowEffect>().GlowBrightness = 2.5f;
        sprite.GetComponent<SpriteGlowEffect>().GlowColor = tower.GetComponent<SpriteGlowEffect>().GlowColor;
    }
}
