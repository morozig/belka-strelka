using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float atackSpeed = 1;
    public GameObject shotPrefab;
    public GameObject bonusPrefab;
    public Vector2 direction;
    public int health;
    public float dropRate;

    private float shootDelay = 10;
    private float nextShootTime;
    private int maxHealth;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerShot>()) {
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
                var dropProb = Random.value;

                if (dropProb <= dropRate) {
                    DropBonus();
                }
            }
        }
    }

    private void Shoot() {
        var platoon = GetComponentInParent<Platoon>();
        if (platoon.state == PlatoonState.Roaming) {
            var time = Time.time;
            if ( nextShootTime <= 0 ) {
                nextShootTime = time + Random.Range(
                    0, 
                    shootDelay / atackSpeed
                );
            }

            if ( time > nextShootTime ) {
                nextShootTime = time + Random.Range(
                    0, 
                    shootDelay / atackSpeed
                );
                var enemyShot = GameObject.Instantiate(
                    shotPrefab,
                    transform.position,
                    transform.rotation
                );

                enemyShot.GetComponent<MoveInDirection>()
                    .direction = direction;
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
}
