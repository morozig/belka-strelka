using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatoonState {
    Idle,
    Entering,
    Roaming,
}

public class Platoon : MonoBehaviour
{
    public PlatoonState state = PlatoonState.Idle;
    public int columnsCount;
    public int rowsCount;
    public Vector2 direction;
    public GameObject childPrefab;

    private float childWidth = 1;
    private float childHeight = 1;
    private float gap = 1;
    private float speed = 1;
    private float enterSpeed = 10;
    private Vector3 nextPoint;
    private float epsilon = 0.1f;
    private Vector2 bottomLeft;
    private Vector2 topRight;

    private float width;
    private float height;

    // Start is called before the first frame update
    void Start()
    {
        width = columnsCount * childWidth + (columnsCount - 1) * gap;
        height = rowsCount * childHeight + (rowsCount - 1) * gap;

        for (int i = 0; i < rowsCount; i++) {
            for (int j = 0; j < columnsCount; j++) {
                var position = (
                    transform.position +
                    new Vector3(
                        - width / 2 + childWidth / 2,
                        height / 2 - childHeight / 2
                    ) +
                    new Vector3(
                        j * (childHeight + gap),
                        - i * (childWidth + gap)
                    )
                );
                var enemy = GameObject.Instantiate(
                    childPrefab,
                    position,
                    transform.rotation,
                    transform
                );
                enemy.GetComponent<Enemy>().direction = direction;
            }
        }
        
        state = PlatoonState.Entering;
        GenerateNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == PlatoonState.Entering) {
            var toPoint = nextPoint - transform.position;
            if (toPoint.magnitude <= epsilon) {
                state = PlatoonState.Roaming;
                GenerateNextPoint();
            } else {
                transform.Translate(
                    toPoint.normalized * enterSpeed * Time.deltaTime,
                    Space.World
                );
            }
        } else if (state == PlatoonState.Roaming) {
            var toPoint = nextPoint - transform.position;
            if (toPoint.magnitude <= epsilon) {
                GenerateNextPoint();
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

    public void SetBounds ( Vector2 bottomLeft, Vector2 topRight) {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
    }
}
