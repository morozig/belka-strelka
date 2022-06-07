using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Player player;
    private bool isShooting;
    private float lastShootTime;
    private float shootDelay = 1;
    private int[] weaponLevelStops = { 1, 2, 5, 10, 20 };

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();    
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    public void OnShootStart() {
        isShooting = true;
    }

    public void OnShootStop() {
        isShooting = false;
    }

    private void Shoot() {
        var weaponAtackSpeeds = player.props.weaponAtackSpeeds;
        var atackSpeed = weaponAtackSpeeds[(int) player.color];
        if (isShooting) {
            var time = Time.time;
            if (
                lastShootTime <= 0 ||
                time - lastShootTime > shootDelay / atackSpeed
            ) {
                lastShootTime = time;
                switch (player.color) {
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

    
    private int GetWeaponLevel() {
        int level = 1;
        for (int i = 0; i < weaponLevelStops.Length; i++) {
            if (player.power >= weaponLevelStops[i]) {
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
        var shotPrefabs = player.props.shotPrefabs;
        var shotPrefab = shotPrefabs[(int) WeaponColor.Red];
        var direction = player.direction;

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
        if (Mathf.Abs(transform.position.x + offset.x) <= 0.5) {
            return;
        }
        var shotPrefabs = player.props.shotPrefabs;
        var shotPrefab = shotPrefabs[(int) WeaponColor.Green];
        var scale = 1 + (damage - 1) * 0.2f;
        var direction = player.direction;
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
        var direction = player.direction;
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
        var direction = player.direction;
        var damage = GetWeaponLevel();
        var scale = damage;
        var offset = new Vector3(
            direction.x,
            direction.y * 5 * 1.28f
        );

        var shotPrefabs = player.props.shotPrefabs;
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
