using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEnemy : MonoBehaviour
{
    public float walkSpeed = 3.0f;

    Rigidbody2D rigitbody;

    TouchingDirections TouchingDirections;

    public enum MovementDirection
    {
        LEFT, RIGHT
    }

    private Vector2 WalkDirectionVector;

    private MovementDirection _walkDirection;
    public MovementDirection WalkDirection
    {
        get { return _walkDirection; }
        set 
        { 
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = reverseXVector(gameObject.transform.localScale);

                WalkDirectionVector = value switch
                {
                    MovementDirection.LEFT => Vector2.left,
                    MovementDirection.RIGHT => Vector2.right
                };
            }

            _walkDirection = value; 
        }
    }

    private void Start()
    {
        WalkDirection = MovementDirection.LEFT;
    }

    private void Awake()
    {
        rigitbody = GetComponent<Rigidbody2D>();
        TouchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        rigitbody.velocity = new Vector2();

        if (TouchingDirections.IsGrounded && TouchingDirections.IsOnWall)
        {
            FlipDirection();
        }

        rigitbody.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, rigitbody.velocity.y);
    }

    private void FlipDirection()
    {
        WalkDirection = WalkDirection switch
        {
            MovementDirection.LEFT => MovementDirection.RIGHT,
            MovementDirection.RIGHT => MovementDirection.LEFT,
            _ => WalkDirection
        };
    }

    private Vector3 reverseXVector(Vector3 pos) => new Vector3(pos.x * -1, pos.y);
}
