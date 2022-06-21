using UnityEngine.UIElements;
using System;

public class UIController
{
    private VisualElement visualElement;

    private TemplateContainer mainMenu;
    private TemplateContainer gameOverlay;
    private GameOverlayController gameOverlayController;

    public UIController(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        mainMenu = visualElement.Q<TemplateContainer>("MainMenu");
        gameOverlay = visualElement.Q<TemplateContainer>("GameOverlay");

        gameOverlayController = new GameOverlayController(gameOverlay);
    }

    public void SetLives(int lives)
    {
        gameOverlayController.SetLives(lives);
    }
}
