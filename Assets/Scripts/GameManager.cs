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
    public GameObject vcamObj;
    public int Level {
        get {
            return levelIndex + 1;
        }
    }
    public int LevelsCount {
        get {
            return levels.Length;
        }
    }

    private SpawnManager spawnManager;
    private AudioSource musicSource;
    private AudioSource crushSource;
    private int levelIndex = 0;
    private int waveIndex = 0;
    private Vcam vcam;

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
        musicSource = music.GetComponent<AudioSource>();
        crushSource = GetComponent<AudioSource>();
        vcam = vcamObj.GetComponent<Vcam>();
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
            musicSource.Stop();
            musicSource.loop = false;
            vcam.StopShake();
        }
    }

    public void DamagePlayer() {
        lives -= 1;
        crushSource.Play();
        vcam.Shake();
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
