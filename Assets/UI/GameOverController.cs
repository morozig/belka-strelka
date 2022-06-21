using UnityEngine.UIElements;
using System;

public class GameOverController
{
    private VisualElement visualElement;
    private Button restartButton;

    public GameOverController(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        restartButton = visualElement.Q<Button>("Restart");
    }

    public void OnRestart(Action onRestart)
    {
        restartButton.clicked += onRestart;
    }
}
