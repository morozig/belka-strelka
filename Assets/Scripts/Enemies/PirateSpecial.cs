using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateSpecial : MonoBehaviour
{
    public GameObject jokerShotPrefab;

    private Enemy enemy;
    private Roaming roaming;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        roaming = GetComponent<Roaming>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void Shoot() {
        if (roaming.state != RoamingState.Idle) {
            return;
        }

        var jokerShot = GameObject.Instantiate(
            jokerShotPrefab,
            transform.position,
            transform.rotation
        );

        jokerShot.GetComponent<MoveInDirection>()
            .direction = enemy.direction;
        roaming.state = RoamingState.Roaming;
    }
}
