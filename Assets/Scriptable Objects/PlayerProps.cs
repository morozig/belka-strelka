using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerProps : ScriptableObject
{
    public float speed = 20;
    public float width = 1;
    public float height = 1;
    public GameObject[] shotPrefabs;
    public float[] weaponAtackSpeeds;
    public float stunSpeed = 1;
    public float stunDuration = 1;
    public AudioClip[] shotSounds;
}
