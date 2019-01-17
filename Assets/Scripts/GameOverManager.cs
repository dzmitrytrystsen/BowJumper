using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Button pauseButton;

    public static GameOverManager instance;

    private GameObject gameOverPanel;
    private Animator gameOverPanelAnimator;

    private Button menuButton, restartButton;

    private Text score;
    private Text bestScore;
    private GameObject powerBar;
    private GameObject scoreText;

	void Awake ()
	{
	    SetUpInstance();

        gameOverPanel = GameObject.Find("GameOverPanelHolder");
	    gameOverPanelAnimator = gameOverPanel.GetComponent<Animator>();

	    menuButton = GameObject.Find("MenuButton").GetComponent<Button>();
	    restartButton = GameObject.Find("RestartButton").GetComponent<Button>();

        menuButton.onClick.AddListener(() => BackToMenu());
        restartButton.onClick.AddListener(() => RestartGame());

        scoreText = GameObject.Find("Score");
	    score = GameObject.Find("FinalScore").GetComponent<Text>();
	    bestScore = GameObject.Find("BestScore").GetComponent<Text>();

	    powerBar = GameObject.Find("PowerBar");

        gameOverPanel.SetActive(false);
	}

    private void SetUpInstance()
    {
        if (instance == null)
            instance = this;
    }

    public void GameOver()
    {
        pauseButton.gameObject.SetActive(false);

        scoreText.SetActive(false);
        powerBar.SetActive(false);

        gameOverPanel.SetActive(true);
        gameOverPanelAnimator.Play("GameOverPanelFade");

        score.text = "" + ScoreManager.instance.GetScore();

        if (ScoreManager.instance.GetScore() > GameController.instance.GetHighscore())
        {
            GameController.instance.SetHighscore(Mathf.RoundToInt(ScoreManager.instance.GetScore()));
        }

        bestScore.text = "" + GameController.instance.GetHighscore();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
