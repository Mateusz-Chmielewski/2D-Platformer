using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class MovingPlatformController : MonoBehaviour
{
    private float startPosition;
    public float moveRange = 50.0f;
    private Animator animator;
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.5f;

    private bool isMovingRight = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        startPosition = (this.transform.position.x);
    }

    // Start is called before the first frame update
    void Update()
    {
        if (this.transform.position.x > startPosition + moveRange)
        {
            isMovingRight = false;
        }
        if (this.transform.position.x < startPosition - moveRange)
        {
            isMovingRight = true;
        }
        if (isMovingRight)
        {
            MoveRight();
        }
        else
        {
            MoveUp();
        }
    }

    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        gameObject.transform.localScale = new Vector3(-1, 1, 1);
    }

    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    void MoveUp()
    {
        transform.Translate(0.0f, moveSpeed * Time.deltaTime, 0.0f, Space.World);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

}
