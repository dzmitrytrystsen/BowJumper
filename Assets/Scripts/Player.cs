using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Rigidbody2D myRigidbody2D;
    public Animator myAnimator;

    [SerializeField] private float jumpPowerX, jumpPowerY, jumpPowerXMax = 6.5f, jumpPowerYMax = 13.5f;
    
    private float tresholdX = 7f;
    private float tresholdY = 14f;

    private bool setPower, didJump;

    void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        SetUpInstance();
    }

    void Update()
    {
        SetPower();
    }

    private void SetUpInstance()
    {
        if (instance == null)
            instance = this;
    }

    private void SetPower()
    {
        if (setPower)
        {
            jumpPowerX += tresholdX * Time.deltaTime;
            jumpPowerY += tresholdY * Time.deltaTime;

            if (jumpPowerX > jumpPowerXMax)
                jumpPowerX = jumpPowerXMax;

            if (jumpPowerY > jumpPowerYMax)
                jumpPowerY = jumpPowerYMax;
        }
    }

    public void SetPower(bool setPower)
    {
        this.setPower = setPower;

        if (!setPower)
        {
            Jump();
        }

        if (setPower)
            Debug.Log("Setting the Power");

        else
            Debug.Log("Not setting the Power");
    }

    private void Jump()
    {
        myRigidbody2D.velocity = new Vector2(jumpPowerX, jumpPowerY);
        myAnimator.SetTrigger("Jump");

        didJump = true;

        jumpPowerX = jumpPowerY = 0f;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (didJump)
        {
            didJump = false;

            if (target.tag == "Platform")
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.CreateNewPlatformAndLerp(target.transform.position.x);
                }
            }
        }

        if (target.tag == "DeathCollider")
        {
            if (GameOverManager.instance != null)
            {
                GameOverManager.instance.GameOver();
            }

            Destroy(gameObject, 0.1f);
        }
    }
}
