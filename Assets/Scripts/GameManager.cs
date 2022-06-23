using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    Victory,
}

public class GameManager : MonoBehaviour
{
    public Level[] levels;
    public int lives = 3;
    public GameState State { get; private set; } = GameState.Idle;
    public GameObject music;

    private SpawnManager spawnManager;
    private AudioSource audioSource;
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
        audioSource = music.GetComponent<AudioSource>();
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
                State = GameState.Victory;
            }
        }

        if (lives <= 0) {
            State = GameState.Over;

            var players = FindObjectsOfType<Player>();

            foreach(var playerObj in players) {
                Destroy(playerObj.gameObject);
            }

            Time.timeScale = 0.2f;
            audioSource.Stop();
            audioSource.loop = false;
        }
    }

    public void DamagePlayer() {
        lives -= 1;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
