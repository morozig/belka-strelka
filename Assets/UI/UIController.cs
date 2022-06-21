using UnityEngine.UIElements;
using System;

public class UIController
{
    private VisualElement visualElement;

    private TemplateContainer gameOverlay;
    private TemplateContainer gameOver;
    private GameOverlayController gameOverlayController;
    private GameOverController gameOverController;

    public UIController(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        gameOverlay = visualElement.Q<TemplateContainer>("GameOverlay");
        gameOver = visualElement.Q<TemplateContainer>("GameOver");

        gameOverlayController = new GameOverlayController(gameOverlay);
        gameOverController = new GameOverController(gameOver);
    }

    public void SetLives(int lives)
    {
        gameOverlayController.SetLives(lives);
    }

    public void OnRestart(Action onRestart)
    {
        gameOverController.OnRestart(onRestart);
    }

    public void GameOver()
    {
        gameOver.style.display = DisplayStyle.Flex;
    }
}
