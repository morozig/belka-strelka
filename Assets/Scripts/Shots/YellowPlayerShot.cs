using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlayerShot : MonoBehaviour
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
        spriteRenderer.sprite = sprites[damage - 1];
    }
}
