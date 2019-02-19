using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static JumpButton instance;

    void Awake()
    {
        SetUpInstance();
    }

    private void SetUpInstance()
    {
        if (instance == null)
            instance = this;
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (Player.instance != null)
            Player.instance.SetPower(true);

        GameController.instance.myAudioSource.PlayOneShot(
            GameController.instance.powerUpAudioClip, .2f);
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (Player.instance != null)
            Player.instance.SetPower(false);

        GameController.instance.myAudioSource.Stop();
        GameController.instance.myAudioSource.PlayOneShot(
            GameController.instance.jumpAudioClip, .5f);
    }
}
