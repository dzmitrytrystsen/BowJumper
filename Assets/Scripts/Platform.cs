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

    private void DestroyPlatform()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        Destroy(gameObject, 2f);
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
                Invoke("DestroyPlatform", 5f);
            }
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
