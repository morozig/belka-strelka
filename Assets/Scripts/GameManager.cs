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
    public GameObject platoonPrefab;
    public Camera mainCamera;
    public bool IsOver {
        get;
        private set;
    }

    private float width;
    private float height;
    private float border = 0.2f;

    private List<GameObject> waveObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        height = mainCamera.orthographicSize * 2;
        width = mainCamera.aspect * height;

        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        var enemiesCount = FindObjectsOfType<Enemy>().Length;
        var enemyShotsCount = FindObjectsOfType<EnemyShot>().Length;
        var bonusesCount = FindObjectsOfType<Bonus>().Length;

        var staffCount = (
            enemiesCount +
            enemyShotsCount +
            bonusesCount
        );
        if (staffCount <= 0) {
            foreach(var gameObject in waveObjects) {
                Destroy(gameObject);
            }
            waveObjects = new List<GameObject>();
            SpawnWave();
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

    private void SpawnWave() {
        var topRightPlatoon = GameObject.Instantiate(
            platoonPrefab,
            GetStartPoint(GameQuadrant.TopRight),
            platoonPrefab.transform.rotation
        );
        var topRightBounds = GetBounds(GameQuadrant.TopRight);
        var topRightScript = topRightPlatoon.GetComponent<Platoon>();
        topRightScript.SetBounds(
            topRightBounds.bottomLeft,
            topRightBounds.topRight
        );
        topRightScript.direction = Vector2.left;
        waveObjects.Add(topRightPlatoon);
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
        waveObjects.Add(bottomLeftPlatoon);
    }

    public void GameOver() {
        Debug.Log("Game Over");
        IsOver = false;
    }
}
