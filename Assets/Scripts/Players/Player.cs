using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 20;
    public float atackSpeed = 2;
    public float width = 1;
    public float height = 1;
    public Vector2 bottomLeft;
    public Vector2 topRight;
    public GameObject shotPrefab;
    public Vector2 direction;

    private bool isShooting;
    private float lastShootTime;
    private float shootDelay = 1;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        BoundMovement();
        Shoot();
    }

    
    private void BoundMovement() {
        var paddedBottomLeft = bottomLeft + new Vector2(
            width / 2,
            height / 2
        );
        var paddedTopRight = topRight - new Vector2(
            width / 2,
            height / 2
        );

        if (transform.position.x < paddedBottomLeft.x) {
            transform.position = new Vector3(
                paddedBottomLeft.x,
                transform.position.y, 
                0
            );
        }
        if (transform.position.x > paddedTopRight.x) {
            transform.position = new Vector3(
                paddedTopRight.x,
                transform.position.y, 
                0
            );
        }
        if (transform.position.y < paddedBottomLeft.y) {
            transform.position = new Vector3(
                transform.position.x,
                paddedBottomLeft.y,
                0
            );
        }
        if (transform.position.y > paddedTopRight.y) {
            transform.position = new Vector3(
                transform.position.x,
                paddedTopRight.y, 
                0
            );
        }
    }

    public void OnShootStart() {
        isShooting = true;
    }

    public void OnShootStop() {
        isShooting = false;
    }

    private void Shoot() {
        if (isShooting) {
            var time = Time.time;
            if (
                lastShootTime <= 0 ||
                time - lastShootTime > shootDelay / atackSpeed
            ) {
                lastShootTime = time;
                var playerShot = GameObject.Instantiate(
                    shotPrefab,
                    transform.position,
                    transform.rotation
                );

                playerShot.GetComponent<MoveInDirection>()
                    .direction = direction;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<EnemyShot>()) {
            Destroy(other.gameObject);
            gameManager.GameOver();
        }
    }
}
