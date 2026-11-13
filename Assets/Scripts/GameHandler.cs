using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameHandler : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    [SerializeField] private ScoreUI scoreUI;

    public bool IsPlaying { get; private set; }
    private bool isGameOver;

    private int score;
    private int highScore;

    private AudioSource audioSource;
    [SerializeField] AudioClip hit;
    [SerializeField] private float playButtonDelay = 1.5f;
    private Coroutine playButtonCoroutine;
    private static bool autoStartNextLoad;

    void OnEnable() => scoreUI.PlayClicked += OnPlayClicked;
    void OnDisable() => scoreUI.PlayClicked -= OnPlayClicked;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        FreezeGame();
        scoreUI.SetHighScore(highScore);
        scoreUI.UpdateScore(0);
        scoreUI.ShowGameOver(false);

        if (autoStartNextLoad)
        {
            autoStartNextLoad = false;
            StartGame();
        }
        else
        {
            scoreUI.ShowPlayButton(true);
        }
    }

    private void OnPlayClicked()
    {
        audioSource.PlayOneShot(hit);

        if (isGameOver)
        {
            autoStartNextLoad = true;
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
            return;
        }

        StartGame();
    }

    private void StartGame()
    {
        IsPlaying = true;
        isGameOver = false;
        score = 0;

        if (playButtonCoroutine != null)
        {
            StopCoroutine(playButtonCoroutine);
            playButtonCoroutine = null;
        }

        scoreUI.UpdateScore(0);
        scoreUI.ShowPlayButton(false);
        scoreUI.ShowGameOver(false);

        Time.timeScale = 1f;
    }

    public void IncScore()
    {
        if (!IsPlaying) return;
        score++;
        scoreUI.UpdateScore(score);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        FreezeGame();

        isGameOver = true;
        IsPlaying = false;

        if (score > highScore)
        {
            highScore = score;

            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }

        scoreUI.ShowHighScore(highScore);
        scoreUI.ShowGameOver(true);
        scoreUI.ShowPlayButton(false);
        if (playButtonCoroutine != null)
            StopCoroutine(playButtonCoroutine);
        playButtonCoroutine = StartCoroutine(ShowPlayButtonAfterDelay());
    }


    private void FreezeGame()
    {
        Time.timeScale = 0f;            
        IsPlaying = false;
        isGameOver = false;
    }

    private IEnumerator ShowPlayButtonAfterDelay()
    {
        yield return new WaitForSecondsRealtime(playButtonDelay);

        scoreUI.ShowGameOver(false);
        scoreUI.ShowPlayButton(true);

        playButtonCoroutine = null;
    }

}