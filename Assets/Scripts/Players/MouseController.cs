using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Player player;
    private Shooter shooter;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        shooter = GetComponent<Shooter>();
    }

    // Update is called once per frame
    void Update()
    {
        var speed = player.speed;
        var mousePosition = Input.mousePosition;
        var unitPixels = Screen.height / mainCamera.orthographicSize / 2;

        var gameMousePosition = new Vector3(
            ( mousePosition.x - Screen.width / 2 ) / unitPixels,
            ( mousePosition.y - Screen.height / 2) / unitPixels,
            0
        );
        
        var delta = gameMousePosition - transform.position;
        var boundDelta = delta.magnitude > 1 ?
            delta.normalized : delta;
        transform.Translate(
            boundDelta * Time.deltaTime * speed,
            Space.World
        );

        if (Input.GetMouseButtonDown(0)) {
            shooter.OnShootStart();
        }

        if (Input.GetMouseButtonUp(0)) {
            shooter.OnShootStop();
        }
    }
}
