using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteGlow;

public class ZoneMeterController : MonoBehaviour
{
    private GameObject sprites;
    
    public GameObject statBar;

    private void Start() {
        statBar = transform.Find("Bar/StatBar").gameObject;
        sprites = transform.Find("Sprites").gameObject;
        Glow(sprites);
    }

    private void Update() {
        statBar.transform.localScale = new Vector2(statBar.transform.localScale.x, statBar.transform.localScale.y - 0.001f);
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
