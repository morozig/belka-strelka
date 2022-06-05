using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float atackSpeed = 1;
    public GameObject shotPrefab;
    public Vector2 direction;
    public int health;

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
            health -= 1;
            spriteRenderer.color = new Color(
                1.0f,
                1.0f * health / maxHealth,
                1.0f * health / maxHealth,
                1.0f
            );
            if (health <= 0) {
                Destroy(gameObject);
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
}
