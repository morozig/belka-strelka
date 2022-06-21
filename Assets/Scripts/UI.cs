using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public GameObject gameManagerObj;

    private UIController uiController;
    private GameManager gameManager;
    private int currentLives = 3;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        uiController = new UIController(uiDocument.rootVisualElement);
        gameManager = gameManagerObj.GetComponent<GameManager>();

        uiController.OnRestart(OnRestart);
    }

    // Update is called once per frame
    void Update()
    {
        var lives = gameManager.lives;

        if (lives != currentLives) {
            uiController.SetLives(lives);
            currentLives = lives;
        }

        var gameState = gameManager.State;

        if (gameState == GameState.Over) {
            uiController.GameOver();
        } else if (gameState == GameState.Victory) {
            uiController.Victory();
        }
    }

    private void OnRestart() {
        gameManager.Restart();
    }
}
