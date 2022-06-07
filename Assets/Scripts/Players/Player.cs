using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WeaponColor {
    Red,
    Green,
    Yellow,
}

public class Player : MonoBehaviour
{
    public float speed = 20;
    public float atackSpeed = 2;
    public float width = 1;
    public float height = 1;
    public Vector2 bottomLeft;
    public Vector2 topRight;
    public Vector2 direction;
    public int power = 1;
    public WeaponColor color = WeaponColor.Red;
    public Sprite[] sprites;
    public GameObject[] shotPrefabs;

    private bool isShooting;
    private float lastShootTime;
    private float shootDelay = 1;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    private int[] weaponLevelStops = { 1, 2, 5, 10, 20 };

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                switch (color) {
                    case WeaponColor.Red: {
                        ShootRed();
                        break;
                    }
                    case WeaponColor.Green: {
                        ShootGreen();
                        break;
                    }
                    case WeaponColor.Yellow: {
                        ShootYellow();
                        break;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<EnemyShot>()) {
            Destroy(other.gameObject);
            gameManager.GameOver();
        }

        if (other.GetComponent<Bonus>()) {
            var bonus = other.GetComponent<Bonus>();
            if (
                bonus.color == BonusColor.Multi ||
                (int) bonus.color == (int) color
            ) {
                power += 1;
            } else {
                color = (WeaponColor) bonus.color;
                power = (int) Math.Floor(power / 2.0f) + 1;
                spriteRenderer.sprite = sprites[(int) color];
            }
            Destroy(other.gameObject);
        }
    }

    private int GetWeaponLevel() {
        int level = 1;
        for (int i = 0; i < weaponLevelStops.Length; i++) {
            if (power >= weaponLevelStops[i]) {
                level = i + 1;
            } else {
                break;
            }
        }
        return level;
    }
    
    private void ShootRedDirection(
        float angle,
        bool isDouble = false
    ) {
        var shotPrefab = shotPrefabs[(int) WeaponColor.Red];
        if (!isDouble) {
            var offset = new Vector3(
                direction.x,
                direction.y
            ) * 0.3f;
            var playerShot = GameObject.Instantiate(
                shotPrefab,
                transform.position + offset,
                Quaternion.Euler(0, 0, angle) * transform.rotation
            );
            playerShot.GetComponent<MoveInDirection>()
                .direction = direction;
        } else {
            var newDirection = Quaternion.Euler(0, 0, angle) *
                direction;
            var offset = new Vector3(
                direction.x,
                direction.y
            ) * 0.3f;
            var offset1 = offset + new Vector3(
                -newDirection.y,
                newDirection.x
            ) * 0.2f;
            var playerShot1 = GameObject.Instantiate(
                shotPrefab,
                transform.position + offset1,
                Quaternion.Euler(0, 0, angle) * transform.rotation
            );
            playerShot1.GetComponent<MoveInDirection>()
                .direction = direction;

            var offset2 = offset + new Vector3(
                newDirection.y,
                -newDirection.x
            ) * 0.2f;
            var playerShot2 = GameObject.Instantiate(
                shotPrefab,
                transform.position + offset2,
                Quaternion.Euler(0, 0, angle) * transform.rotation
            );
            playerShot2.GetComponent<MoveInDirection>()
                .direction = direction;

        }
    }
    private void ShootRed() {
        var level = GetWeaponLevel();
        switch (level) {
            case 1: {
                ShootRedDirection(0);
                break;
            }
            case 2: {
                ShootRedDirection(-10);
                ShootRedDirection(10);
                break;
            }
            case 3: {
                ShootRedDirection(-30);
                ShootRedDirection(-10);
                ShootRedDirection(10);
                ShootRedDirection(30);
                break;
            }
            case 4: {
                ShootRedDirection(-30, true);
                ShootRedDirection(-10, true);
                ShootRedDirection(10, true);
                ShootRedDirection(30, true);
                break;
            }
            case 5: {
                ShootRedDirection(-45, true);
                ShootRedDirection(-22, true);
                ShootRedDirection(0, true);
                ShootRedDirection(22, true);
                ShootRedDirection(45, true);
                break;
            }
        }
    }

    private void ShootGreenLine(
        Vector3 offset,
        int damage = 1
    ) {
        if (Math.Abs(transform.position.x + offset.x) <= 0.5) {
            return;
        }

        var scale = 1 + (damage - 1) * 0.2f;

        var shotPrefab = shotPrefabs[(int) WeaponColor.Green];
        var playerShot = GameObject.Instantiate(
            shotPrefab,
            transform.position + offset,
            transform.rotation
        );
        playerShot.transform.localScale = new Vector3(
            scale,
            scale
        );
        playerShot.GetComponent<MoveInDirection>()
            .direction = direction;
        playerShot.GetComponent<PlayerShot>().damage = damage;

    }
    private void ShootGreen() {
        var level = GetWeaponLevel();
        switch (level) {
            case 1: {
                ShootGreenLine(direction * 0.9f);
                break;
            }
            case 2: {
                ShootGreenLine(new Vector2(-0.9f, 0));
                ShootGreenLine(new Vector2(0.9f, 0));
                break;
            }
            case 3: {
                ShootGreenLine(new Vector2(-0.9f, 0));
                ShootGreenLine(direction * 0.9f);
                ShootGreenLine(new Vector2(0.9f, 0));
                break;
            }
            case 4: {
                ShootGreenLine(new Vector2(-0.9f, 0), 2);
                ShootGreenLine(direction * 0.9f, 2);
                ShootGreenLine(new Vector2(0.9f, 0), 2);
                break;
            }
            case 5: {
                ShootGreenLine(new Vector2(-0.9f, 0), 4);
                ShootGreenLine(direction * 0.9f, 4);
                ShootGreenLine(new Vector2(0.9f, 0), 4);
                break;
            }
        }
    }

    private void ShootYellow() {
        var damage = GetWeaponLevel();
        var scale = damage;
        var offset = new Vector3(
            direction.x,
            direction.y * 5 * 1.28f
        );

        var shotPrefab = shotPrefabs[(int) WeaponColor.Yellow];
        var playerShot = GameObject.Instantiate(
            shotPrefab,
            transform.position + offset,
            transform.rotation
        );
        playerShot.transform.localScale = new Vector3(
            playerShot.transform.localScale.x * scale,
            playerShot.transform.localScale.y
        );
        playerShot.GetComponent<MoveInDirection>()
            .direction = direction;
        playerShot.GetComponent<PlayerShot>().damage = damage;
    }
}
