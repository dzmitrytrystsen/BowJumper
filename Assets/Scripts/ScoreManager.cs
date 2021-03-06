﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private Text scoreText;

    public float score;

	void Awake ()
	{
	    scoreText = GameObject.Find("Score").GetComponent<Text>();
	    SetUpInstance();
	}

    private void SetUpInstance()
    {
        if (instance == null)
            instance = this;
    }

    public void AddScore()
    {
        score++;
        scoreText.text = "" + score;
    }

    public float GetScore() { return this.score; }
}
