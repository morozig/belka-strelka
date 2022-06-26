using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bonusPrefab;
    public Vector2 direction;
    public int health;
    public float dropRate;
    public GameObject platoonObj;
    public bool flipRight;
    public GameObject explosionPrefab;

    private int maxHealth;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (flipRight && Vector2.Dot(direction, Vector2.right) == 1) {
            spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerShot>()) {
            if (gameObject != null) {
                var playerShot = other.GetComponent<PlayerShot>();
                health -= playerShot.damage;
                if (health > 0) {
                    spriteRenderer.color = new Color(
                        1.0f,
                        1.0f * health / maxHealth,
                        1.0f * health / maxHealth,
                        1.0f
                    );
                } else {
                    Destroy(gameObject);
                    Explode();

                    var dropProb = Random.value;

                    if (dropProb <= dropRate) {
                        DropBonus();
                    }
                }
            } else {
                Debug.Log("Already destroyed");
            }
        }
    }

    private void DropBonus() {
        var bonus = GameObject.Instantiate(
            bonusPrefab,
            transform.position,
            transform.rotation
        );

        bonus.GetComponent<MoveInDirection>()
            .direction = direction;
    }

    private void Explode() {
        GameObject.Instantiate(
            explosionPrefab,
            transform.position,
            explosionPrefab.transform.rotation
        );
    }
}
