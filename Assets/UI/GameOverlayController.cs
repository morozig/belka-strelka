using UnityEngine.UIElements;

public class GameOverlayController
{
    private VisualElement visualElement;
    private Label livesLabel;

    private int livesLenth = 2;

    public GameOverlayController(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        livesLabel = visualElement.Q<Label>("Lives");
    }

    public void SetLives(int lives)
    {
        livesLabel.text = lives.ToString($"D{livesLenth}");
    }
}
