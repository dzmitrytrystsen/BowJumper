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

        Player.instance.myAudioSource.PlayOneShot(Player.instance.powerUpAudioClip);
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (Player.instance != null)
            Player.instance.SetPower(false);

        Player.instance.myAudioSource.Stop();
        Player.instance.myAudioSource.PlayOneShot(Player.instance.JumpAudioClip);
    }
}
