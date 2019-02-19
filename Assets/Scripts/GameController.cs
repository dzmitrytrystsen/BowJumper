using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] public AudioClip landingAudioClip, powerUpAudioClip, deathAudioClip, jumpAudioClip;
    [SerializeField] public Sprite[] muteButtonSprites;

    public bool isGameMuted = false;

    private Button muteButton;

    private AudioSource mainThemeAudioSource;

    public AudioSource myAudioSource;


    public static GameController instance;

    private const string highScore = "High Score";

	void Awake()
	{
		SetUpSingleton();
        IsTheGameStartedForTheFirstTime();

	    myAudioSource = GetComponent<AudioSource>();
        mainThemeAudioSource = GameObject.Find("MainTheme").GetComponent<AudioSource>();
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

    public void MuteSound()
    {
        if (myAudioSource.mute && mainThemeAudioSource.mute)
        {
            myAudioSource.mute = false;
            mainThemeAudioSource.mute = false;

            muteButton = GameObject.Find("MuteButton").GetComponent<Button>();
            muteButton.GetComponent<Image>().sprite = muteButtonSprites[0];

            isGameMuted = false;
        }

        else
        {
            myAudioSource.mute = true;
            mainThemeAudioSource.mute = true;
            muteButton = GameObject.Find("MuteButton").GetComponent<Button>();
            muteButton.GetComponent<Image>().sprite = muteButtonSprites[1];

            isGameMuted = true;
        }
    }
}
