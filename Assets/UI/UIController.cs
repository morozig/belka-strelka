using UnityEngine.UIElements;

public class UIController
{
    private VisualElement visualElement;

    private TemplateContainer gameOverlay;
    private GameOverlayController gameOverlayController;

    public UIController(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        gameOverlay = visualElement.Q<TemplateContainer>("GameOverlay");
        gameOverlayController = new GameOverlayController(gameOverlay);
    }

    public void SetLives(string lives)
    {
        gameOverlayController.SetLives(lives);
    }
}
