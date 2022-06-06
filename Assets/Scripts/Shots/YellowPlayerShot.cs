using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlayerShot : MonoBehaviour
{
    public float timeToLive = 0.2f;
    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        var time = Time.time;

        if (time - spawnTime > timeToLive) {
            Destroy(gameObject);
        }
    }
}
