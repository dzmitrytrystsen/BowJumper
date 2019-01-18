using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] platform;
    [SerializeField] public int platformsAmount;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;

    [SerializeField] private GameObject mountains;

    private float minX = -2f, maxX = 2f, minY = -4f, maxY = -1.5f;

    private bool lerpCamera;
    private float lerpTime = 1.5f;
    private float lerpX;

    void Awake()
    {
        SetUpInstance();
        CreateInitialPlatform();
    }

    void Update()
    {
        if (lerpCamera)
            LerpCamera();
    }

    private void SetUpInstance()
    {
        if (instance == null)
            instance = this;
    }

    public void AddPlatform()
    {
        platformsAmount++;
    }

    public void SubstractPlatform()
    {
        platformsAmount--;
    }

    void CreateInitialPlatform()
    {
        Vector2 temp = new Vector2(Random.Range(minX, minX + 1.5f), Random.Range(minY, maxY));

        Instantiate(platform[Random.Range(0, platform.Length)], temp, Quaternion.identity);

        temp.y += 5f;

        AddPlatform();

        Instantiate(player, temp, Quaternion.identity);

        temp = new Vector2(Random.Range(maxX, maxX - 1f), Random.Range(minY, maxY));

        Instantiate(platform[Random.Range(0, platform.Length)], temp, Quaternion.identity);

        AddPlatform();
    }

    public void CreateNewPlatformAndLerp(float lerpPosition)
    {
        if (platformsAmount <= 2)
        {
            CreateNewPlatform();
        }

        lerpX = lerpPosition + maxX;
        lerpCamera = true;
    }

    void CreateNewPlatform()
    {
        float cameraX = Camera.main.transform.position.x;
        float newMaxX = (maxX * 2f) + cameraX;

        Vector2 temp = new Vector2(Random.Range(newMaxX, newMaxX - 0.3f),
            Random.Range(maxY, maxY - 0.5f));

        Instantiate(platform[Random.Range(0, platform.Length)], temp, Quaternion.identity);

        AddPlatform();
    }

    void LerpCamera()
    {
        float x = Camera.main.transform.position.x;

        x = Mathf.Lerp(x, lerpX, lerpTime * Time.deltaTime);

        Camera.main.transform.position =
            new Vector3(x, Camera.main.transform.position.y, Camera.main.transform.position.z);

        x = Mathf.Lerp(mountains.transform.position.x, lerpX, lerpTime * Time.deltaTime * 0.5f);

        mountains.transform.position =
            new Vector3(x, mountains.transform.position.y, mountains.transform.position.z);


        if (Camera.main.transform.position.x >= (lerpX - 0.1f))
        {
            lerpCamera = false;
        }
    }

    public void PauseGame()
    {
        pauseButton.gameObject.SetActive(false);
        JumpButton.instance.gameObject.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ResumeGame()
    {
        pauseButton.gameObject.SetActive(true);
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}

