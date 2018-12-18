using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator myAnimator;
    private Collider2D myCollider2D;
    private Rigidbody2D myRigidbody2D;

    [SerializeField] private float enemySpeed = 2f;

    private bool isDead = false;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        EnemyMove();
    }

    private void EnemyMove()
    {
        if (IsFacingRight())
        {
            myRigidbody2D.velocity = new Vector2(enemySpeed, 0f);
        }

        else
        {
            myRigidbody2D.velocity = new Vector2(-enemySpeed, 0f);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Arrow")
        {
            Death();
        }
    }

    private void Death()
    {
        enemySpeed = 0f;

        myAnimator.SetTrigger("Dead");
        myAnimator.SetBool("DeadIdle", true);

        Destroy(gameObject, 5f);
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Platform")
        {
            transform.localScale = new Vector2(-Mathf.Sign(myRigidbody2D.velocity.x), 1f);
        }
    }
}
