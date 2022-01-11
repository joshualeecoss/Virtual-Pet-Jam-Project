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

    public meterType type;

    private GameObject sprites;

    private void Start() {
        sprites = transform.Find("Sprites").gameObject;
        
        if (type == meterType.Hunger) {
            var tower = GameObject.Find("HungerTower");
            for (int i = 0; i < sprites.transform.childCount; i++) {
                var sprite = sprites.transform.GetChild(i).gameObject;
                ChangeGlow(tower, sprite);
            }
        } else if (type == meterType.Sleep) {
            var tower = GameObject.Find("SleepTower");
            for (int i = 0; i < sprites.transform.childCount; i++) {
                var sprite = sprites.transform.GetChild(i).gameObject;
                ChangeGlow(tower, sprite);
            }
        } else if (type == meterType.Stamina) {
            var tower = GameObject.Find("StaminaTower");
            for (int i = 0; i < sprites.transform.childCount; i++) {
                var sprite = sprites.transform.GetChild(i).gameObject;
                ChangeGlow(tower, sprite);
            }
        } else if (type == meterType.Thirst) {
            var tower = GameObject.Find("ThirstTower");
            for (int i = 0; i < sprites.transform.childCount; i++) {
                var sprite = sprites.transform.GetChild(i).gameObject;
                ChangeGlow(tower, sprite);
            }
        }
    }

    private void ChangeGlow(GameObject tower, GameObject sprite) {
        sprite.AddComponent<SpriteGlowEffect>();
        sprite.GetComponent<SpriteGlowEffect>().GlowBrightness = 2.5f;
        sprite.GetComponent<SpriteGlowEffect>().GlowColor = tower.GetComponent<SpriteGlowEffect>().GlowColor;
    }
}
