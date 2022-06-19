using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlayerShot : MonoBehaviour
{
    private PlayerShot playerShot;
    private SpriteRenderer spriteRenderer;

    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        playerShot = GetComponent<PlayerShot>();
        var damage = playerShot.damage;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[DamageToSpriteIndex(damage)];
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Enemy>()) {
            var enemy = other.GetComponent<Enemy>();
            if (playerShot.damage < enemy.health) {
                Destroy(gameObject);
            }
        }
    }

    private int DamageToSpriteIndex(int damage) {
        switch (damage) {
            case 1: {
                return 0;
            }
            case 2: {
                return 1;
            }
            case 4: {
                return 2;
            }
            default: {
                return 0;
            }
        }
    }
}
