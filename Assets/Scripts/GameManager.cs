using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameQuadrant {
    TopLeft,
    TopRight,
    BottomRight,
    BottomLeft,
}

public class GameManager : MonoBehaviour
{
    public bool IsOver { get; private set; }
    public Level[] levels;

    private SpawnManager spawnManager;
    private int levelIndex;
    private int waveIndex;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GetComponent<SpawnManager>();

        spawnManager.SpawnWave(levels[levelIndex].waves[waveIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOver && !spawnManager.IsActive) {
            waveIndex += 1;
            if (waveIndex >= levels[levelIndex].waves.Length) {
                waveIndex = 0;
                levelIndex += 1;
            }
            if (levelIndex < levels.Length) {
                spawnManager.SpawnWave(levels[levelIndex].waves[waveIndex]);
            } else {
                Debug.Log("Victory!");
                IsOver = true;
            }
        }
    }

    public void GameOver() {
        Debug.Log("Game Over");
        IsOver = false;
    }
}
