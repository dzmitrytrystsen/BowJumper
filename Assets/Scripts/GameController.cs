using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    private const string highScore = "High Score";

	void Awake()
	{
		SetUpSingleton();
        IsTheGameStartedForTheFirstTime();
	}

    private void SetUpSingleton()
    {
        if (instance != null)
            Destroy(gameObject);

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void IsTheGameStartedForTheFirstTime()
    {
        if (!PlayerPrefs.HasKey("IsTheGameStartedForTheFirstTime"))
        {
            PlayerPrefs.SetInt(highScore, 0);
            PlayerPrefs.SetInt("IsTheGameStartedForTheFirstTime", 0);
        }
    }

    public void SetHighscore(int score)
    {
        PlayerPrefs.SetInt(highScore, score);
    }

    public int GetHighscore()
    {
        return PlayerPrefs.GetInt(highScore);
    }
}
