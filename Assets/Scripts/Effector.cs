using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector : MonoBehaviour
{
    public static Effector instance;
    private float score;
    private float windForce;
    private float difficulty = 10f;

    void Start()
    {
        SetUpInstance();

        score = ScoreManager.instance.score;
    }

    private void SetUpInstance()
    {
        if (instance == null)
            instance = this;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            if (windForce <= 55f)
            {
                windForce = GetComponent<AreaEffector2D>().forceMagnitude += score / difficulty;
            }

            else
            {
                windForce = 55f;
            }
        }
    }
}
