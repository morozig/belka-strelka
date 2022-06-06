using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    public float speed = 1;
    public Vector2 direction;

    private float horizontalBound = 31;
    private float verticalBound = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);

        if (
            transform.position.x > horizontalBound ||
            transform.position.x < -horizontalBound ||
            transform.position.y > verticalBound ||
            transform.position.y < -verticalBound
        ) {
            Destroy(gameObject);
        }
    }
}
