using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject platoonPrefab;

    private List<GameObject> platoons = new List<GameObject>();
    public bool IsActive { get; private set; }

    private float width;
    private float height;
    private float border = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        height = mainCamera.orthographicSize * 2;
        width = mainCamera.aspect * height;
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
                foreach(var gameObject in platoons) {
                    Destroy(gameObject);
                }
                platoons.Clear();
                IsActive = false;
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
                    width / 2,
                    height / 2
                );
                return (
                    bottomLeft,
                    topRight
                );
            }
            case GameQuadrant.BottomLeft: {
                var bottomLeft = new Vector2(
                    - width / 2,
                    - height / 2
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

    private Vector3 GetStartPoint (GameQuadrant quadrant) {
        switch (quadrant) {
            case GameQuadrant.TopRight: {
                return new Vector3(
                    Random.Range(0, width / 2),
                    Random.Range(height * 0.75f, height)
                );
            }
            case GameQuadrant.BottomLeft: {
                return new Vector3(
                    Random.Range(- width / 2, 0),
                    Random.Range(- height, - height * 0.75f)
                );
            }
            default: {
                return new Vector3();
            }
        }
    }

    public void SpawnWave(Wave wave) {
        IsActive = true;

        var topRightPlatoon = GameObject.Instantiate(
            platoonPrefab,
            GetStartPoint(GameQuadrant.TopRight),
            platoonPrefab.transform.rotation
        );
        var topRightBounds = GetBounds(GameQuadrant.TopRight);
        var topRightScript = topRightPlatoon.GetComponent<Platoon>();
        topRightScript.direction = Vector2.left;
        topRightScript.childPrefab = wave.platoonUnitPrefab;
        topRightScript.columnsCount = wave.platoonColumns;
        topRightScript.rowsCount = wave.platoonRows;
        topRightScript.SetBounds(
            topRightBounds.bottomLeft,
            topRightBounds.topRight
        );

        platoons.Add(topRightPlatoon);

        var bottomLeftPlatoon = GameObject.Instantiate(
            platoonPrefab,
            GetStartPoint(GameQuadrant.BottomLeft),
            Quaternion.Inverse(platoonPrefab.transform.rotation)
        );
        var bottomLeftBounds = GetBounds(GameQuadrant.BottomLeft);
        var bottomLeftScript = bottomLeftPlatoon.GetComponent<Platoon>();
        bottomLeftScript.SetBounds(
            bottomLeftBounds.bottomLeft,
            bottomLeftBounds.topRight
        );
        bottomLeftScript.direction = Vector2.right;
        bottomLeftScript.childPrefab = wave.platoonUnitPrefab;
        bottomLeftScript.columnsCount = wave.platoonColumns;
        bottomLeftScript.rowsCount = wave.platoonRows;
        bottomLeftScript.SetBounds(
            bottomLeftBounds.bottomLeft,
            bottomLeftBounds.topRight
        );
        platoons.Add(bottomLeftPlatoon);
    }
}
