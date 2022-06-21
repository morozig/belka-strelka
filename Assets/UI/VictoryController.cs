using UnityEngine.UIElements;
using System;

public class VictoryController
{
    private VisualElement visualElement;
    private Button restartButton;

    public VictoryController(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        restartButton = visualElement.Q<Button>("Restart");
    }

    public void OnRestart(Action onRestart)
    {
        restartButton.clicked += onRestart;
    }
}
