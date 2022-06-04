using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float atackSpeed = 1;
    public GameObject shotPrefab;
    public Vector2 direction;

    private float shootDelay = 10;
    private float nextShootTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerShot>()) {
            Destroy(gameObject);
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
