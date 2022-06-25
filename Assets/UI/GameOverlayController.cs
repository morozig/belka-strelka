using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class GameOverlayController
{
    private VisualElement visualElement;
    private Label livesLabel;
    private VisualElement controlsElement;
    private Label levelLabel;

    private int livesLenth = 2;

    public GameOverlayController(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        livesLabel = visualElement.Q<Label>("Lives");
        controlsElement = visualElement.Q<VisualElement>("Controls");
        levelLabel = visualElement.Q<Label>("Level");

        FadeControls();
    }

    public void SetLives(int lives)
    {
        livesLabel.text = lives.ToString($"D{livesLenth}");
    }

    private void FadeControls()
    {
        controlsElement.experimental.animation.Start(1, 0, 5000, (element, val) => {
            element.style.opacity = val;
        }).Ease(Easing.InCubic);
    }

    public void StartLevel(int level, int of)
    {
        levelLabel.text = ($"Level    {level} / {of}");
        levelLabel.experimental.animation.Start(1, 0, 5000, (element, val) => {
            element.style.opacity = val;
        }).Ease(Easing.InCubic);
    }
}
