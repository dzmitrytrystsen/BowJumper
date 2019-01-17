using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField] private GameObject cloud;

    private float minSpeed = -1f;
    private float maxSpeed = -5f;

	void Start()
	{
	}
	
	void Update()
	{
		cloudMovement();
	}

    private void cloudMovement()
    {
    }
}
