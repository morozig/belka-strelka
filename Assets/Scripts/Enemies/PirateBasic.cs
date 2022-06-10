using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PirateBasicState {
    Idle,
    Joining,
    Leaving,
    Serving,
}

public class PirateBasic : MonoBehaviour
{
    public GameObject shotPrefab;
    public PirateBasicState state = PirateBasicState.Idle;
    public float joinSpeed = 3;
    public float leaveSpeed = 5;
    public float shootDelay = 20;
    public float leaveDistance = 5;

    private Enemy enemy;
    private float nextShootTime;

    private Vector3 platoonPosition;
    private float epsilon = 0.1f;
    private Vector3 leaveToPosition;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        var roaming = enemy.platoonObj.GetComponent<Roaming>();

        if (state == PirateBasicState.Idle) {
            if (roaming.state == RoamingState.Roaming) {
                state = PirateBasicState.Serving;
            }
        }
        Join();
        Leave();
        Shoot();
    }

    private void Join() {
        if (state == PirateBasicState.Joining) {
            var toPosition = (
                enemy.platoonObj.transform.position
                + platoonPosition
                - transform.position
            );
            if (toPosition.magnitude <= epsilon) {
                state = PirateBasicState.Idle;
            } else {
                transform.Translate(
                    toPosition.normalized * joinSpeed * Time.deltaTime
                );
            }
        }
    }

    private void Leave() {
        if (state == PirateBasicState.Leaving) {
            var toPosition = (
                leaveToPosition
                - transform.position
            );
            if (toPosition.magnitude <= epsilon) {
                state = PirateBasicState.Joining;
            } else {
                transform.Translate(
                    toPosition.normalized * leaveSpeed * Time.deltaTime
                );
            }
        }
    }

    private void Shoot() {
        if (state == PirateBasicState.Serving) {
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
                
                leaveToPosition = transform.position -
                    new Vector3(
                        enemy.direction.x,
                        enemy.direction.y
                    ) * leaveDistance;
                var platoon = enemy.platoonObj.GetComponent<Platoon>();
                platoon.Leave(gameObject);
                platoonPosition = platoon.Join(gameObject);
                state = PirateBasicState.Leaving;
            }
        }
    }
}
