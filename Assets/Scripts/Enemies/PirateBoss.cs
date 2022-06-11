using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateBoss : MonoBehaviour
{
    public GameObject pirateBasicPrefab;
    public Vector3[] exits;
    public GameObject shotPrefab;

    private Enemy enemy;
    private Roaming roaming;
    private bool isFlipped;
    private float minAngle = -45;
    private float maxAngle = 45;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        roaming = GetComponent<Roaming>();
        isFlipped = Vector2.Dot(enemy.direction, Vector2.right) == 1;
    }

    // Update is called once per frame
    void Update()
    {
        Act();
    }

    void Act() {
        if (roaming.state != RoamingState.Idle) {
            return;
        }
        var platoonObj = enemy.platoonObj;
        var platoon = platoonObj.GetComponent<Platoon>();

        if (platoon.emptyCount < 4) {
            Shoot();
        } else {
            var action = Random.Range(0, 2);
            if (action != 0) {
                Shoot();
            } else {
                Spawn();
            }
        }
        roaming.state = RoamingState.Roaming;
    }

    void SpawnPirate(Vector3 offset) {
        var platoonObj = enemy.platoonObj;
        var platoon = platoonObj.GetComponent<Platoon>();
        var unitObj = GameObject.Instantiate(
            pirateBasicPrefab,
            transform.position + offset,
            transform.rotation,
            platoonObj.transform
        );
        var unitEnemy = unitObj.GetComponent<Enemy>();
        unitEnemy.direction = enemy.direction;
        unitEnemy.platoonObj = platoonObj;
        unitEnemy.dropRate = 0;
        var position = platoon.Join(unitObj);
        var pirateBasic = unitObj.GetComponent<PirateBasic>();
        pirateBasic.JoinPlatoon(position);
    }

    void Spawn() {
        foreach (Vector3 exit in exits) {
            var offset = new Vector3(
                isFlipped ? -exit.x : exit.x,
                exit.y
            );
            SpawnPirate(offset);
        }
    }

    void ShootAngle(
        float angle,
        Vector3 offset
    ) {
        var shot = GameObject.Instantiate(
            shotPrefab,
            transform.position + offset,
            Quaternion.Euler(0, 0, angle) * transform.rotation
        );

        shot.GetComponent<MoveInDirection>()
            .direction = enemy.direction;
        var ricochet = shot.AddComponent<Ricochet>();
        ricochet.top = roaming.topRight.y;
        ricochet.bottom = roaming.bottomLeft.y;
    }

    void Shoot() {
        var angle = minAngle + Random.Range(1, maxAngle - minAngle);
        foreach (Vector3 exit in exits) {
            var offset = new Vector3(
                isFlipped ? -exit.x : exit.x,
                exit.y
            );
            ShootAngle(angle, offset);
        }
    }
}
