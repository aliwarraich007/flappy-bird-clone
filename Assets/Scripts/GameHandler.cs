using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    [SerializeField] private ScoreUI scoreUI;

    public bool IsPlaying { get; private set; }
    private bool isGameOver;

    private int score;
    private int highScore;

    void OnEnable() => scoreUI.PlayClicked += OnPlayClicked;
    void OnDisable() => scoreUI.PlayClicked -= OnPlayClicked;

    void Start()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        Time.timeScale = 0f;            // freeze everything until Play
        IsPlaying = false;
        isGameOver = false;

        scoreUI.UpdateScore(0);
        scoreUI.ShowPlayButton(true);
    }

    private void OnPlayClicked()
    {
        Debug.Log("Play clicked");      // remove once it works

        if (isGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        StartGame();
    }

    private void StartGame()
    {
        IsPlaying = true;
        score = 0;

        scoreUI.UpdateScore(0);
        scoreUI.HideHighScore();
        scoreUI.ShowPlayButton(false);

        Time.timeScale = 1f;            // unfreeze
    }

    public void IncScore()
    {
        if (!IsPlaying) return;
        score++;
        scoreUI.UpdateScore(score);
    }

    public void GameOver()
    {
        if (isGameOver) return;         // guard against repeated collisions
        isGameOver = true;
        IsPlaying = false;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }

        scoreUI.ShowHighScore(highScore);
        scoreUI.ShowPlayButton(true);
    }
}