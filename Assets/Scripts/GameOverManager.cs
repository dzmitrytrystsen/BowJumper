using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    private GameObject gameOverPanel;
    private Animator gameOverPanelAnimator;

    private Button menuButton, restartButton;

    private Text score;
    private GameObject scorePanel;
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

        scorePanel = GameObject.Find("ScorePanel");
        scoreText = GameObject.Find("Score");
	    score = GameObject.Find("FinalScore").GetComponent<Text>();

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
        scoreText.SetActive(false);
        scorePanel.SetActive(false);
        powerBar.SetActive(false);

        gameOverPanel.SetActive(true);
        gameOverPanelAnimator.Play("GameOverPanelFade");

        score.text = "" + ScoreManager.instance.GetScore();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    void Update ()
	{
		
	}
}
