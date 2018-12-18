using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject[] enemy;

    [SerializeField] public int platformsAmount;


    private float minX = -2f, maxX = 2f, minY = -4f, maxY = -3f;

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

        Instantiate(platform, temp, Quaternion.identity);

        temp.y += 5f;

        AddPlatform();

        Instantiate(player, temp, Quaternion.identity);

        temp = new Vector2(Random.Range(maxX, maxX - 1f), Random.Range(minY, maxY));

        Instantiate(platform, temp, Quaternion.identity);

        temp.y += 1f;

        Instantiate(enemy[Random.Range(0, enemy.Length)], temp, Quaternion.identity);

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

        Instantiate(platform, temp, Quaternion.identity);

        temp.y += 1f;
        Instantiate(enemy[Random.Range(0, enemy.Length)], temp, Quaternion.identity);


        AddPlatform();
    }

    void LerpCamera()
    {
        float x = Camera.main.transform.position.x;

        x = Mathf.Lerp(x, lerpX, lerpTime * Time.deltaTime);

        Camera.main.transform.position =
            new Vector3(x, Camera.main.transform.position.y, Camera.main.transform.position.z);

        if (Camera.main.transform.position.x >= (lerpX - 0.1f))
        {
            lerpCamera = false;
        }
    }

    //public void SlowTime(float duration, float speed)
    //{
    //    //1 = normal speed, 2 = double speed, 0 = standstill. 0.5f = half speed etc
    //    Time.timeScale = speed;
    //    StartCoroutine(ResumeNormalSpeed(duration));
    //}

    //public IEnumerator ResumeNormalSpeed(float duration)
    //{
    //    yield return new WaitForSeconds(duration * Time.deltaTime);
    //    Time.timeScale = 1f;
    //}
}

