using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameQuadrant {
    TopLeft,
    TopRight,
    BottomRight,
    BottomLeft,
}

public enum GameState {
    Idle,
    Running,
    Over,
}

public class GameManager : MonoBehaviour
{
    public Level[] levels;
    public int lives = 3;
    public GameState State { get; private set; } = GameState.Idle;

    private SpawnManager spawnManager;
    private int levelIndex = 0;
    private int waveIndex = 0;

    private void Initialize() {
        Random.Range(0, 2);
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GetComponent<SpawnManager>();
        Initialize();
        State = GameState.Running;
        spawnManager.SpawnWave(levels[levelIndex].waves[waveIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        if (State == GameState.Running && !spawnManager.IsActive) {
            waveIndex += 1;
            if (waveIndex >= levels[levelIndex].waves.Length) {
                waveIndex = 0;
                levelIndex += 1;
            }
            if (levelIndex < levels.Length) {
                spawnManager.SpawnWave(levels[levelIndex].waves[waveIndex]);
            } else {
                Debug.Log("Victory!");
                State = GameState.Over;
            }
        }
    }

    public void DamagePlayer() {
        lives -= 1;
        if (lives <= 0) {
            Debug.Log("Game Over");
            State = GameState.Over;
        }
    }
}
