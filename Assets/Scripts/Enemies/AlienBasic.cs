using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitBasicState {
    Idle,
    Joining,
    Serving,
}

public class AlienBasic : MonoBehaviour
{
    public GameObject shotPrefab;
    public UnitBasicState state = UnitBasicState.Idle;
    public float joinSpeed = 3;

    private Enemy enemy;
    private float shootDelay = 10;
    private float nextShootTime;

    private Vector3 platoonPosition;
    private float epsilon = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        var roaming = enemy.platoonObj.GetComponent<Roaming>();

        if (state == UnitBasicState.Idle) {
            if (roaming.state == RoamingState.Roaming) {
                state = UnitBasicState.Serving;
            }
        }
        Join();
        Shoot();
    }

    private void Join() {
        if (state == UnitBasicState.Joining) {
            var toPosition = (
                enemy.platoonObj.transform.position
                + platoonPosition
                - transform.position
            );
            if (toPosition.magnitude <= epsilon) {
                state = UnitBasicState.Idle;
            } else {
                transform.Translate(
                    toPosition.normalized * joinSpeed * Time.deltaTime
                );
            }
        }
    }

    private void Shoot() {
        if (state == UnitBasicState.Serving) {
            var time = Time.time;
            if ( nextShootTime <= 0 ) {
                nextShootTime = time + Random.Range(
                    0, 
                    shootDelay
                );
            }

            if ( time > nextShootTime ) {
                nextShootTime = time + Random.Range(
                    0, 
                    shootDelay
                );
                var enemyShot = GameObject.Instantiate(
                    shotPrefab,
                    transform.position,
                    transform.rotation
                );

                enemyShot.GetComponent<MoveInDirection>()
                    .direction = enemy.direction;
            }
        }
    }

    public void JoinPlatoon(Vector3 position) {
        platoonPosition = position;
        state = UnitBasicState.Joining;
    }
}
