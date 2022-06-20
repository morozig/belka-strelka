using UnityEngine.UIElements;
using System;

public class UIController
{
    private VisualElement visualElement;

    private TemplateContainer mainMenu;
    private TemplateContainer gameOverlay;
    private MainMenuController mainMenuController;
    private GameOverlayController gameOverlayController;

    public UIController(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        mainMenu = visualElement.Q<TemplateContainer>("MainMenu");
        gameOverlay = visualElement.Q<TemplateContainer>("GameOverlay");

        mainMenuController = new MainMenuController(mainMenu);
        gameOverlayController = new GameOverlayController(gameOverlay);

        mainMenuController.OnStart(OnClickStart);
    }

    public void SetLives(int lives)
    {
        gameOverlayController.SetLives(lives);
    }

    public void OnStart(Action cb)
    {
        mainMenuController.OnStart(cb);
    }

    private void OnClickStart()
    {
        mainMenu.style.display = DisplayStyle.None;
        gameOverlay.style.display = DisplayStyle.Flex;
    }
}
