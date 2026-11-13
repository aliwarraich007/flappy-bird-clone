using UnityEngine;
using UnityEngine.UIElements;
using System;

[RequireComponent(typeof(PanelRenderer))]
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Sprite[] digits;

    private PanelRenderer panelRenderer;
    private VisualElement scoreContainer;
    private VisualElement highScoreContainer;
    private VisualElement highScorePanel;
    private VisualElement gameOverElement;
    private bool gameOverVisible;
    public event Action PlayClicked;
    private Button playButton;
    private bool playVisible = true;

    private int currentScore;
    private int currentHighScore;

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
        gameOverElement = root.Q<VisualElement>("game-over");
        playButton = root.Q<Button>("play-btn");

        if (playButton != null)
        {
            playButton.clicked -= OnPlayButtonClicked;
            playButton.clicked += OnPlayButtonClicked;
        }

        Refresh();
    }


    private void OnPlayButtonClicked()
    {
        PlayClicked?.Invoke();
    }


    public void SetHighScore(int highScore)
    {
        currentHighScore = highScore;
        if (highScoreContainer != null)
            SetNumber(highScoreContainer, currentHighScore);
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
        Refresh();
    }

    private void Refresh()
    {
        if (scoreContainer != null)
            SetNumber(scoreContainer, currentScore);

        if (highScoreContainer != null)
            SetNumber(highScoreContainer, currentHighScore);

        if (playButton != null)
            playButton.style.display = playVisible
                ? DisplayStyle.Flex
                : DisplayStyle.None;

        if (gameOverElement != null)
            gameOverElement.style.display = gameOverVisible
                ? DisplayStyle.Flex
                : DisplayStyle.None;

        if (highScorePanel != null)
            highScorePanel.style.display = DisplayStyle.Flex;
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
    public void ShowGameOver(bool show)
    {
        gameOverVisible = show;
        Refresh();
    }
}