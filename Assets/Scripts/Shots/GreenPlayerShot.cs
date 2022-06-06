using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlayerShot : MonoBehaviour
{
    private PlayerShot playerShot;

    // Start is called before the first frame update
    void Start()
    {
        playerShot = GetComponent<PlayerShot>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Enemy>()) {
            var enemy = other.GetComponent<Enemy>();
            if (playerShot.damage < enemy.health) {
                Destroy(gameObject);
            }
        }
    }
}
