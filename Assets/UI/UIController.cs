using UnityEngine.UIElements;
using System;

public class UIController
{
    private VisualElement visualElement;

    private TemplateContainer gameOverlay;
    private TemplateContainer gameOver;
    private TemplateContainer victory;
    private GameOverlayController gameOverlayController;
    private GameOverController gameOverController;
    private VictoryController victoryController;

    public UIController(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        gameOverlay = visualElement.Q<TemplateContainer>("GameOverlay");
        gameOver = visualElement.Q<TemplateContainer>("GameOver");
        victory = visualElement.Q<TemplateContainer>("Victory");

        gameOverlayController = new GameOverlayController(gameOverlay);
        gameOverController = new GameOverController(gameOver);
        victoryController = new VictoryController(victory);
    }

    public void SetLives(int lives)
    {
        gameOverlayController.SetLives(lives);
    }

    public void OnRestart(Action onRestart)
    {
        gameOverController.OnRestart(onRestart);
        victoryController.OnRestart(onRestart);
    }

    public void GameOver()
    {
        gameOver.style.display = DisplayStyle.Flex;
    }

    public void Victory()
    {
        victory.style.display = DisplayStyle.Flex;
    }
}
