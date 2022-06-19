using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BonusColor {
    Red,
    Green,
    Yellow,
    Multi,
}

public class Bonus : MonoBehaviour
{
    public BonusColor color;
    public Sprite[] sprites;
    public bool isMirrored;

    private SpriteRenderer spriteRenderer;
    private MoveInDirection moveInDirection;
    private float horizontalBound = 17.7f;
    private bool isBoosted;

    // Start is called before the first frame update
    void Start()
    {
        var colorIndex = Random.Range(0, 4);
        color = (BonusColor) colorIndex;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[(int) color];
        moveInDirection = GetComponent<MoveInDirection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (
            !isMirrored && (
                transform.position.x > horizontalBound ||
                transform.position.x < -horizontalBound
            )
        ) {
            moveInDirection.direction *= -1;
            moveInDirection.speed = 5;
            var mirroredPosition = new Vector3(
                transform.position.x,
                -transform.position.y
            );
            transform.position = mirroredPosition;
            isMirrored = true;
        }
    }

    public void BoostSpeed() {
        if (!isBoosted) {
            isBoosted = true;
            if (!isMirrored) {
                moveInDirection.speed = 4;
            }
        }
    }
}
