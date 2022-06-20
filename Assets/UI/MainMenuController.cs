using UnityEngine.UIElements;
using System;

public class MainMenuController
{
    private VisualElement visualElement;
    private Button startButton;

    public MainMenuController(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        startButton = visualElement.Q<Button>("Start");
    }

    public void OnStart(Action cb)
    {
        startButton.clicked += cb;
    }
}
