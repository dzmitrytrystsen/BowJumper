﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Slider = UnityEngine.UI.Slider;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Rigidbody2D myRigidbody2D;
    public Animator myAnimator;

    [SerializeField] private float jumpPowerX, jumpPowerY, jumpPowerXMax = 6.5f, jumpPowerYMax = 13.5f;
    [SerializeField] private bool ifCanJump;

    private float tresholdX = 7f;
    private float tresholdY = 14f;

    public bool setPower, didJump;

    private Slider powerBar;
    private float powerBarTreshold = 10f;
    private float powerBarValue = 0f;

    private GameObject jumpButton;

    void Awake()
    {
        Initialization();
        SetUpInstance();
    }

    void Start()
    {
        jumpButton.SetActive(false);
        myAnimator.SetBool("isFalling", true);
    }

    private void Initialization()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        powerBar = GameObject.Find("PowerBar").GetComponent<Slider>();
        jumpButton = GameObject.Find("JumpButton");

        powerBar.minValue = 0f;
        powerBar.maxValue = 10f;
        powerBar.value = powerBarValue;
    }

    void Update()
    {
        if (ifCanJump)
        {
            SetPower();
        }
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
            jumpPowerX += tresholdX * 2.5f * Time.deltaTime;
            jumpPowerY += tresholdY * 2.5f * Time.deltaTime;

            if (jumpPowerX > jumpPowerXMax)
                jumpPowerX = jumpPowerXMax;

            if (jumpPowerY > jumpPowerYMax)
                jumpPowerY = jumpPowerYMax;

            powerBarValue += powerBarTreshold * Time.deltaTime;
            powerBar.value = powerBarValue;
        }
    }

    public void SetPower(bool setPower)
    {
        this.setPower = setPower;

        if (!setPower)
        {
            Jump();
        }
    }

    private void Jump()
    {
        myRigidbody2D.velocity = new Vector2(jumpPowerX, jumpPowerY);
        myAnimator.SetTrigger("Jump");
        myAnimator.SetBool("isFalling", true);

        didJump = true;

        jumpPowerX = jumpPowerY = 0f;

        powerBarValue = 0f;
        powerBar.value = powerBarValue;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (didJump)
        {
            didJump = false;
        }

        if (target.tag == "Platform")
        {
            transform.localScale = new Vector2(0.5f, 0.5f);

            myAnimator.SetBool("isFalling", false);
        }

        if (target.tag == "EffectorCollider")
        {
            myAnimator.SetBool("isFalling", false);
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

    void OnTriggerStay2D(Collider2D target)
    {
        if (target.tag == "Platform")
        {
            ifCanJump = true;
            jumpButton.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Platform")
        {
            ifCanJump = false;
            jumpButton.SetActive(false);
        }
    }
}
