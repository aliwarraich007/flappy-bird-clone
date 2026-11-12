using UnityEngine;
using UnityEngine.UIElements;
using System; // for Action

[RequireComponent(typeof(PanelRenderer))]
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Sprite[] digits; // index 0-9

    private PanelRenderer panelRenderer;
    private VisualElement scoreContainer;
    private VisualElement highScoreContainer;
    private VisualElement highScorePanel;
    public event Action PlayClicked;
    private Button playButton;
    private bool playVisible = true;

    // cached state, reapplied whenever the UI reloads
    private int currentScore;
    private int currentHighScore;
    private bool highScoreVisible;

    private void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReload);
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        scoreContainer = root.Q<VisualElement>("score-container");
        highScoreContainer = root.Q<VisualElement>("highscore-container");
        highScorePanel = root.Q<VisualElement>("highscore-panel");

        playButton = root.Q<Button>("play-btn");
        playButton.clicked += () => PlayClicked?.Invoke();

        Refresh();
    }

    public void UpdateScore(int score)
    {
        currentScore = score;
        if (scoreContainer != null)
            SetNumber(scoreContainer, currentScore);
    }

    public void ShowHighScore(int highScore)
    {
        currentHighScore = highScore;
        highScoreVisible = true;
        Refresh();
    }

    public void HideHighScore()
    {
        highScoreVisible = false;
        Refresh();
    }

    private void Refresh()
    {
        if (scoreContainer == null) return;

        SetNumber(scoreContainer, currentScore);
        SetNumber(highScoreContainer, currentHighScore);
        highScorePanel.style.display =
            highScoreVisible ? DisplayStyle.Flex : DisplayStyle.None;

        playButton.style.display = playVisible ? DisplayStyle.Flex : DisplayStyle.None;

    }

    private void SetNumber(VisualElement container, int value)
    {
        container.Clear();

        foreach (char c in value.ToString())
        {
            var digit = new VisualElement();
            digit.AddToClassList("digit");
            digit.style.backgroundImage = new StyleBackground(digits[c - '0']);
            container.Add(digit);
        }
    }

    public void ShowPlayButton(bool show)
    {
        playVisible = show;
        Refresh();
    }
}