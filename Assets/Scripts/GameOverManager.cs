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

	void Awake ()
	{
	    SetUpInstance();

        gameOverPanel = GameObject.Find("GameOverPanelHolder");
	    gameOverPanelAnimator = gameOverPanel.GetComponent<Animator>();

	    menuButton = GameObject.Find("MenuButton").GetComponent<Button>();
	    restartButton = GameObject.Find("RestartButton").GetComponent<Button>();

        menuButton.onClick.AddListener(() => BackToMenu());
        restartButton.onClick.AddListener(() => RestartGame());

	    score = GameObject.Find("Score").GetComponent<Text>();

        gameOverPanel.SetActive(false);
	}

    private void SetUpInstance()
    {
        if (instance == null)
            instance = this;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverPanelAnimator.Play("GameOverPanelFade");
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
