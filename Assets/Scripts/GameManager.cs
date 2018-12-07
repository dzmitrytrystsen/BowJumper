using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject platform;

    private float minX = -2f, maxX = 2f, minY = -4.5f, maxY = -2f;

    private bool lerpCamera;
    private float lerpTime = 1.5f;
    private float lerpX;

	void Awake ()
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

    void CreateInitialPlatform()
    {
        Vector2 temp = new Vector2(Random.Range(minX, minX + 1f), Random.Range(minY, maxY));

        Instantiate(platform, temp, Quaternion.identity);

        temp.y += 2f;

        Instantiate(player, temp, Quaternion.identity);

        temp = new Vector2(Random.Range(maxX, maxX - 1f), Random.Range(minY, maxY));

        Instantiate(platform, temp, Quaternion.identity);
    }

    public void CreateNewPlatformAndLerp(float lerpPosition)
    {
        CreateNewPlatform();

        lerpX = lerpPosition + maxX;
        lerpCamera = true;
    }

    void CreateNewPlatform()
    {
        float cameraX = Camera.main.transform.position.x;
        float newMaxX = (maxX * 2f) + cameraX;

        Instantiate(platform, new Vector2(Random.Range(newMaxX, newMaxX - 1f),
                                        Random.Range(maxY, maxY - 1f)), Quaternion.identity);
    }

    void LerpCamera()
    {
        float x = Camera.main.transform.position.x;

        x = Mathf.Lerp(x, lerpX, lerpTime * Time.deltaTime);

        Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y, Camera.main.transform.position.z);

        if (Camera.main.transform.position.x >= (lerpX - 0.1f))
        {
            lerpCamera = false;
        }
    }
}
