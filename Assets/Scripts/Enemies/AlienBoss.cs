using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBoss : MonoBehaviour
{
    public GameObject shotPrefab;

    private Enemy enemy;
    private Roaming roaming;
    private float minAngle = -45;
    private float maxAngle = 45;

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

    private void ShootAngle(
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
    private void Shoot() {
        if (roaming.state != RoamingState.Idle) {
            return;
        }
        
        var angle1 = minAngle + Random.Range(1, maxAngle - minAngle);
        ShootAngle(
            angle1,
            new Vector2(
                0,
                1.5f
            )
        );
        var angle2 = minAngle + Random.Range(1, maxAngle - minAngle);
        ShootAngle(
            angle2,
            new Vector2(
                0,
                -1.5f
            )
        );

        roaming.state = RoamingState.Roaming;
    }
}
