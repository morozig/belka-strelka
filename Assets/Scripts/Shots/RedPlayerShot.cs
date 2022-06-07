using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RedPlayerShot : MonoBehaviour
{
    private int initialSide;
    
    // Start is called before the first frame update
    void Start()
    {
        initialSide = (int) Mathf.Sign(transform.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        var currentSide = (int) Mathf.Sign(transform.position.x);
        if (currentSide != initialSide) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Enemy>()) {
            Destroy(gameObject);
        }
    }
}
