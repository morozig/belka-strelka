using UnityEngine.UIElements;

public class GameOverlayController
{
    private VisualElement visualElement;
    private Label livesLabel;

    public GameOverlayController(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        livesLabel = visualElement.Q<Label>("Lives");
    }

    public void SetLives(string lives)
    {
        livesLabel.text = lives;
    }
}
