using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSpecial : MonoBehaviour
{
    public GameObject basicPrefab;

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
        Spawn();
    }

    void Spawn() {
        if (roaming.state != RoamingState.Idle) {
            return;
        }
        var platoonObj = enemy.platoonObj;
        var platoon = platoonObj.GetComponent<Platoon>();
        if (platoon.isFull) {
            roaming.state = RoamingState.Roaming;
            return;
        }
        var unitObj = GameObject.Instantiate(
            basicPrefab,
            transform.position,
            transform.rotation,
            platoonObj.transform
        );
        var unitEnemy = unitObj.GetComponent<Enemy>();
        unitEnemy.direction = enemy.direction;
        unitEnemy.platoonObj = platoonObj;
        unitEnemy.dropRate = 0;
        var position = platoon.Join(unitObj);
        var alienBasic = unitObj.GetComponent<AlienBasic>();
        alienBasic.JoinPlatoon(position);
        roaming.state = RoamingState.Roaming;
    }
}
