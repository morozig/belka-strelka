using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    private Player player;
    private Shooter shooter;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        shooter = GetComponent<Shooter>();
    }

    // Update is called once per frame
    void Update()
    {
        var speed = player.props.speed;
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        
        transform.Translate(
            new Vector2(
                horizontalInput * Time.deltaTime,
                verticalInput * Time.deltaTime
            ) * speed,
            Space.World
        );

        if (
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.LeftShift)
        ) {
            shooter.OnShootStart();
        }
        if (
            Input.GetKeyUp(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.LeftShift)
        ) {
            shooter.OnShootStop();
        }
    }
}
