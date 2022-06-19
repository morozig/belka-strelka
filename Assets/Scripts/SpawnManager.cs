using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject platoonPrefab;

    private List<GameObject> platoons = new List<GameObject>();
    public bool IsActive { get; private set; }

    private float _width;
    private float _height;
    private float border = 0.2f;
    private float aspect = 16f / 9f;

    private bool isInitialized;
    private bool isWaveSpawning;

    private float Width {
        get {
            if (isInitialized) {
                return _width;
            } else {
                Initialize();
                return _width;
            }
        }
    }
    private float Height {
        get {
            if (isInitialized) {
                return _height;
            } else {
                Initialize();
                return _height;
            }
        }
    }

    private void Initialize() {
        if (!isInitialized) {
            mainCamera.aspect = aspect;
            _height = mainCamera.orthographicSize * 2;
            _width = aspect * _height;
            isInitialized = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive) {
            var enemiesCount = FindObjectsOfType<Enemy>().Length;
            var enemyShotsCount = FindObjectsOfType<EnemyShot>().Length;
            var bonusesCount = FindObjectsOfType<Bonus>().Length;

            var staffCount = (
                enemiesCount +
                enemyShotsCount +
                bonusesCount
            );

            if (staffCount <= 0) {
                if (!isWaveSpawning) {
                    foreach(var gameObject in platoons) {
                        Destroy(gameObject);
                    }
                    platoons.Clear();
                    IsActive = false;
                }
            } else {
                isWaveSpawning = false;
            }
        }
    }
    
    private (
        Vector2 bottomLeft,
        Vector2 topRight
    ) GetBounds (GameQuadrant quadrant) {
        switch (quadrant) {
            case GameQuadrant.TopRight: {
                var bottomLeft = new Vector2(
                    border / 2,
                    border / 2
                );
                var topRight = new Vector2(
                    Width / 2,
                    Height / 2
                );
                return (
                    bottomLeft,
                    topRight
                );
            }
            case GameQuadrant.BottomLeft: {
                var bottomLeft = new Vector2(
                    - Width / 2,
                    - Height / 2
                );
                var topRight = new Vector2(
                    - border / 2,
                    - border / 2
                );
                return (
                    bottomLeft,
                    topRight
                );
            }
            default: {
                var bottomLeft = new Vector2();
                var topRight = new Vector2();
                return (
                    bottomLeft,
                    topRight
                );
            }
        }
    }

    private Vector2 GetDirection (GameQuadrant quadrant) {
        switch (quadrant) {
            case GameQuadrant.TopRight: 
            case GameQuadrant.BottomRight: 
                return Vector2.left;
            case GameQuadrant.BottomLeft:
            case GameQuadrant.TopLeft:
                return Vector2.right;
            default:
                return new Vector2();
        }
    }

    private Vector3 GetStartPoint (GameQuadrant quadrant) {
        switch (quadrant) {
            case GameQuadrant.TopRight: {
                return new Vector3(
                    Random.Range(0, Width / 2),
                    Random.Range(Height * 0.75f, Height)
                );
            }
            case GameQuadrant.BottomLeft: {
                return new Vector3(
                    Random.Range(- Width / 2, 0),
                    Random.Range(- Height, - Height * 0.75f)
                );
            }
            default: {
                return new Vector3();
            }
        }
    }

    private GameObject SpawnQuadrant(
        Wave wave,
        GameQuadrant gameQuadrant
    ) {
        var bounds = GetBounds(gameQuadrant);
        var direction = GetDirection(gameQuadrant);
        GameObject platoonObj = null;
        if (platoonPrefab) {
            platoonObj = GameObject.Instantiate(
                platoonPrefab,
                GetStartPoint(gameQuadrant),
                platoonPrefab.transform.rotation
            );
            var platoon = platoonObj.GetComponent<Platoon>();
            platoon.direction = direction;
            platoon.childPrefab = wave.platoonUnitPrefab;
            platoon.columnsCount = wave.platoonColumns;
            platoon.rowsCount = wave.platoonRows;
            platoon.isFull = wave.isPlatoonFull;
            var platoonRoaming = platoonObj.GetComponent<Roaming>();
            platoonRoaming.bottomLeft = bounds.bottomLeft;
            platoonRoaming.topRight = bounds.topRight;
        }

        if (wave.roamingUnitsCount > 0) {
            for (var i = 0; i < wave.roamingUnitsCount; i++) {
                var unitObj = GameObject.Instantiate(
                    wave.roamingUnitPrefab,
                    GetStartPoint(gameQuadrant),
                    wave.roamingUnitPrefab.transform.rotation
                );
                var enemy = unitObj.GetComponent<Enemy>();
                enemy.direction = direction;
                enemy.platoonObj = platoonObj;
                var unitRoaming = unitObj.GetComponent<Roaming>();
                unitRoaming.bottomLeft = bounds.bottomLeft;
                unitRoaming.topRight = bounds.topRight;
            }
        }
        return platoonObj;
    }

    public void SpawnWave(Wave wave) {
        var topRightPlatoon = SpawnQuadrant(
            wave,
            GameQuadrant.TopRight
        );
        platoons.Add(topRightPlatoon);
        var bottomLeftPlatoon = SpawnQuadrant(
            wave,
            GameQuadrant.BottomLeft
        );
        platoons.Add(bottomLeftPlatoon);
        isWaveSpawning = true;
        IsActive = true;
    }
}
