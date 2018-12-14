using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator myAnimator;
    private Collider2D myCollider2D;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();
    }


    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Arrow")
        {
            myAnimator.SetTrigger("Dead");
            myAnimator.SetBool("DeadIdle", true);

            Destroy(gameObject, 5f);
        }
    }
}
