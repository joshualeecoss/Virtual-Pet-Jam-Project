using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteGlow;

public class ZoneMeterController : MonoBehaviour
{
    private GameObject sprites;

    private void Start() {
        sprites = transform.Find("Sprites").gameObject;
        Glow(sprites);
    }

    private void Glow(GameObject sprites) {
        for (int i = 0; i < sprites.transform.childCount; i++) {
            var sprite = sprites.transform.GetChild(i).gameObject;
            sprite.AddComponent<SpriteGlowEffect>();
            sprite.GetComponent<SpriteGlowEffect>().GlowBrightness = 2.5f;
            sprite.GetComponent<SpriteGlowEffect>().GlowColor = Color.red;
        }
    }
}
