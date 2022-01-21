using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteGlow;

public class StatMeterController : MonoBehaviour
{
    private const float DECAY_RATE = 0.00005f;
    public const float FULL_TIMEOUT = 5.0f;
    public enum meterType {
        Hunger,
        Sleep,
        Stamina,
        Thirst
    }

    public enum STATE {
        Full,
        Decreasing,
        Stopped
    }

    public STATE state;
    private float decayRate;


    public float maxValue, currentValue, zoneThreshold;
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

        decayRate = 0f;

        maxValue = statBar.transform.localScale.y;
        currentValue = maxValue;
        zoneThreshold = maxValue * 0.75f;

        state = STATE.Full;
    }

    private void FixedUpdate() {
        switch (state) {
            case STATE.Full:
                HandleFull();
                break;
            case STATE.Decreasing:
                HandleDecreasing();
                break;
            case STATE.Stopped:
                HandleStopped();
                break;
        }  
    }

    private void HandleFull() {
        StartCoroutine(FullTimeout());
    }

    private void HandleDecreasing() {
        if (currentValue >= 0) {
            currentValue -= decayRate;
            statBar.transform.localScale = new Vector2(statBar.transform.localScale.x, currentValue);
        }  else {
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

    private IEnumerator FullTimeout() {
        yield return new WaitForSeconds(FULL_TIMEOUT);
        state = STATE.Decreasing;
    }

    public float GetCurrentValue() {
        return currentValue;
    }

    public float GetMaxValue() {
        return maxValue;
    }

    public void Fill() {
        currentValue = maxValue;
    }

    public float GetThreshold() {
        return zoneThreshold;
    }

    public void IncreaseDecayRate() {
        decayRate += DECAY_RATE;
    }

}
