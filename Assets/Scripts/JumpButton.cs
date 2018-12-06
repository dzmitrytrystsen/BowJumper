using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData data)
    {
        if (Player.instance != null)
            Player.instance.SetPower(true);

        Debug.Log("Touching the Button");
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (Player.instance != null)
            Player.instance.SetPower(false);


        Debug.Log("Have relesed the Button");
    }
}
