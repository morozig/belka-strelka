using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : MonoBehaviour
{
    public float top;
    public float bottom;

    private MoveInDirection moveInDirection;
    private CircleCollider2D circleCollider2D;
    private float paddetTop;
    private float paddetBottom;

    // Start is called before the first frame update
    void Start()
    {
        moveInDirection = GetComponent<MoveInDirection>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        var radius = circleCollider2D.radius;
        paddetTop = top - radius;
        paddetBottom = bottom + radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (
            transform.position.y >= paddetTop ||
            transform.position.y <= paddetBottom
        ) {
            var angle = transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }
}
