using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBoss : MonoBehaviour
{
    public GameObject shotPrefab;

    private Enemy enemy;
    private Roaming roaming;
    private float minAngle = -45;
    private float maxAngle = 45;
    private int shotCount = 8;
    private float shotDelay = 0.2f;
    private float shotAngleRange = 10;
    private Coroutine shootingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        roaming = GetComponent<Roaming>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void ShootAngle(float angle) {
        var shot = GameObject.Instantiate(
            shotPrefab,
            transform.position,
            Quaternion.Euler(0, 0, angle) * transform.rotation
        );

        shot.GetComponent<MoveInDirection>()
            .direction = enemy.direction;
        
        var ricochet = shot.AddComponent<Ricochet>();
        ricochet.top = roaming.topRight.y;
        ricochet.bottom = roaming.bottomLeft.y;
    }

    private IEnumerator ShootBurst() {
        var baseAngle = minAngle + Random.Range(1, maxAngle - minAngle);

        for (int i = 0; i < shotCount; i++) {
            var angle = baseAngle + Random.Range(
                -shotAngleRange,
                shotAngleRange
            );
            ShootAngle(angle);
            yield return new WaitForSeconds(shotDelay);
        }
        shootingCoroutine = null;
        roaming.state = RoamingState.Roaming;
    }

    private void Shoot() {
        if (roaming.state == RoamingState.Idle) {
            if (shootingCoroutine == null) {
                shootingCoroutine = StartCoroutine(ShootBurst());
            }
        }
    }
}
