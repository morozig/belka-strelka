using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponColor {
    Red,
    Green,
    Yellow,
}

public class Player : MonoBehaviour
{
    public PlayerProps props;
    public Vector2 bottomLeft;
    public Vector2 topRight;
    public Vector2 direction;
    public int power = 1;
    public WeaponColor color = WeaponColor.Red;
    public Sprite[] sprites;
    public bool isStunned;
    public float speed;
    public GameObject gameManagerObj;

    private SpriteRenderer spriteRenderer;
    private float stunTime;
    private int greyIndex = 3;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = props.speed;
    }

    // Update is called once per frame
    void Update()
    {
        BoundMovement();
        Stunned();
    }

    private void BoundMovement() {
        var width = props.width;
        var height = props.height;
        var paddedBottomLeft = bottomLeft + new Vector2(
            width / 2,
            height / 2
        );
        var paddedTopRight = topRight - new Vector2(
            width / 2,
            height / 2
        );

        if (transform.position.x < paddedBottomLeft.x) {
            transform.position = new Vector3(
                paddedBottomLeft.x,
                transform.position.y, 
                0
            );
        }
        if (transform.position.x > paddedTopRight.x) {
            transform.position = new Vector3(
                paddedTopRight.x,
                transform.position.y, 
                0
            );
        }
        if (transform.position.y < paddedBottomLeft.y) {
            transform.position = new Vector3(
                transform.position.x,
                paddedBottomLeft.y,
                0
            );
        }
        if (transform.position.y > paddedTopRight.y) {
            transform.position = new Vector3(
                transform.position.x,
                paddedTopRight.y, 
                0
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<EnemyShot>()) {
            Destroy(other.gameObject);

            power = Mathf.Max(
                (int) Mathf.Floor(power / 2.0f),
                1
            );

            gameManager.DamagePlayer();
        }

        if (other.GetComponent<Bonus>()) {
            // Debug.Log("Was " + color + " int " + (int) color);
            var bonus = other.GetComponent<Bonus>();
            // Debug.Log("Bonus " + bonus.color + " int " + (int) bonus.color);
            if (
                bonus.color == BonusColor.Multi ||
                (int) bonus.color == (int) color
            ) {
                power += 1;
            } else {
                color = (WeaponColor) bonus.color;
                power = (int) Mathf.Floor(power / 2.0f) + 1;
                spriteRenderer.sprite = sprites[(int) color];
            }
            // Debug.Log("Became " + color + " int " + (int) color);
            Destroy(other.gameObject);
        }

        if (other.GetComponent<JokerShot>()) {
            var jokerShot = other.GetComponent<JokerShot>();
            if (jokerShot.type == JokerType.Bomb) {
                Destroy(other.gameObject);
                gameManager.DamagePlayer();
            } else {
                if (
                    jokerShot.type == JokerType.BonusMulti ||
                    (int) jokerShot.type == (int) color
                ) {
                    power += 1;
                } else {
                    color = (WeaponColor) jokerShot.type;
                    power = (int) Mathf.Floor(power / 2.0f) + 1;
                    spriteRenderer.sprite = sprites[(int) color];
                }
                Destroy(other.gameObject);
            }
        }

        if (other.GetComponent<StunShot>()) {
            Destroy(other.gameObject);
            isStunned = true;
            stunTime = Time.time;
            spriteRenderer.sprite = sprites[greyIndex];
            speed = props.stunSpeed;
        }
    }

    private void Stunned() {
        if (isStunned) {
            if ((Time.time - stunTime) > props.stunDuration) {
                isStunned = false;
                stunTime = -props.stunDuration;
                spriteRenderer.sprite = sprites[(int) color];
                speed = props.speed;
            }
        }
    }
}
