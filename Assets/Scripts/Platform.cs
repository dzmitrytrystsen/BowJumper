using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static Platform instance;

    private Animator myAnimator;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "PlatformKiller")
        {
            Destroy(gameObject);
            GameManager.instance.SubstractPlatform();
        }

        if (target.tag == "Player")
        {
            if (GameManager.instance != null && Player.instance.didJump)
            {
                ScoreManager.instance.AddScore();
                myAnimator.SetTrigger("Destroy");
                GameManager.instance.CreateNewPlatformAndLerp(target.transform.position.x);
            }

            if (ScoreManager.instance != null && Player.instance.didJump)
            {
                Destroy(gameObject, 5f);
            }
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
