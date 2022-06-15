using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RobotBasicState {
    Idle,
    Joining,
    Roaming,
    Serving,
}

public class RobotBasic : MonoBehaviour
{
    public GameObject shotPrefab;
    public GameObject stunPrefab;
    public RobotBasicState state = RobotBasicState.Idle;
    public float joinSpeed = 3;
    public float roamSpeed = 2;
    public float shootDelay = 20;
    public float width = 1;
    public float height = 1;
    public Vector2 bottomLeft;
    public Vector2 topRight;

    private Enemy enemy;
    private float nextShootTime;
    private int maxHealth;

    private Vector3 platoonPosition;
    private float epsilon = 0.1f;
    private Vector3 nextPoint;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        maxHealth = enemy.health;
        var roaming = enemy.platoonObj.GetComponent<Roaming>();
        bottomLeft = roaming.bottomLeft;
        topRight = roaming.topRight;
    }

    // Update is called once per frame
    void Update()
    {
        var roaming = enemy.platoonObj.GetComponent<Roaming>();
        if (state == RobotBasicState.Idle) {
            if (roaming.state == RoamingState.Roaming) {
                state = RobotBasicState.Serving;
            }
        }

        Leave();
        Join();
        Shoot();
        Roam();
    }

    
    private void Leave() {
        if (state == RobotBasicState.Serving) {
            if (enemy.health < maxHealth) {
                var platoon = enemy.platoonObj.GetComponent<Platoon>();
                platoon.Leave(gameObject);
                transform.parent = null;
                GenerateNextPoint();
                state = RobotBasicState.Roaming;
            }
        }
    }

    private void Join() {
        if (state == RobotBasicState.Joining) {
            var toPosition = (
                enemy.platoonObj.transform.position
                + platoonPosition
                - transform.position
            );
            if (toPosition.magnitude <= epsilon) {
                state = RobotBasicState.Idle;
            } else {
                transform.Translate(
                    toPosition.normalized * joinSpeed * Time.deltaTime
                );
            }
        }
    }

    private void Shoot() {
        if (state == RobotBasicState.Serving) {
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

    private void Roam() {
        if (state == RobotBasicState.Roaming) {
            var toPoint = nextPoint - transform.position;
            if (toPoint.magnitude <= epsilon) {
                GenerateNextPoint();
                var stun = GameObject.Instantiate(
                    stunPrefab,
                    transform.position,
                    transform.rotation
                );

                stun.GetComponent<MoveInDirection>()
                    .direction = enemy.direction;
            } else {
                transform.Translate(
                    toPoint.normalized * roamSpeed * Time.deltaTime,
                    Space.World
                );
            }
        }
    }

    private void GenerateNextPoint () {
        var paddedBottomLeft = bottomLeft + new Vector2(
            width / 2,
            height / 2
        );
        var paddedTopRight = topRight - new Vector2(
            width / 2,
            height / 2
        );
        nextPoint = new Vector3(
            Random.Range(paddedBottomLeft.x, paddedTopRight.x),
            Random.Range(paddedBottomLeft.y, paddedTopRight.y)
        );
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<RobotSpecial>()) {
            if (enemy.health < maxHealth) {
                enemy.health = maxHealth;
                var platoon = enemy.platoonObj.GetComponent<Platoon>();
                transform.parent = enemy.platoonObj.transform;
                platoonPosition = platoon.Join(gameObject);

                var spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(
                    1.0f,
                    1.0f,
                    1.0f,
                    1.0f
                );
                state = RobotBasicState.Joining;
            }
        }
    }
}
