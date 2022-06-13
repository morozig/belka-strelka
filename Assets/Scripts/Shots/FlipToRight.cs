using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipToRight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private MoveInDirection moveInDirection;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveInDirection = GetComponent<MoveInDirection>();

        if (Vector2.Dot(moveInDirection.direction, Vector2.right) == 1) {
            spriteRenderer.flipX = true;
        }
    }
}
