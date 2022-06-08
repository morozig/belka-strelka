using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoamingState {
    Idle,
    Entering,
    Roaming,
}

public class Roaming : MonoBehaviour
{
    public RoamingState state  = RoamingState.Idle;
    public float speed = 1;
    public float width;
    public float height;
    public Vector2 bottomLeft;
    public Vector2 topRight;
    public bool idleOnStop;

    private float enterSpeed = 10;
    private Vector3 nextPoint;
    private float epsilon = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        state = RoamingState.Entering;
        GenerateNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == RoamingState.Entering) {
            var toPoint = nextPoint - transform.position;
            if (toPoint.magnitude <= epsilon) {
                state = RoamingState.Roaming;
                GenerateNextPoint();
            } else {
                transform.Translate(
                    toPoint.normalized * enterSpeed * Time.deltaTime,
                    Space.World
                );
            }
        } else if (state == RoamingState.Roaming) {
            var toPoint = nextPoint - transform.position;
            if (toPoint.magnitude <= epsilon) {
                GenerateNextPoint();
                if (idleOnStop) {
                    state = RoamingState.Idle;
                }
            } else {
                transform.Translate(
                    toPoint.normalized * speed * Time.deltaTime,
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
}
