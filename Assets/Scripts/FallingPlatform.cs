using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    BoxCollider2D boxCollider;
    SpriteRenderer sprite;
    private bool isFalling = false;
    private Vector2 startPosition;
    private float modifier = 0.1f;

    void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        startPosition = transform.position;
    }

    IEnumerator Disable()
    {
        isFalling = true;
        yield return new WaitForSeconds(1);
        boxCollider.isTrigger = true;
        sprite.enabled = false;
        isFalling = false;
        transform.position = startPosition;
        yield return new WaitForSeconds(3);
        boxCollider.isTrigger = false;
        sprite.enabled = true;  
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            StartCoroutine(Disable());
        }
    }

    void FixedUpdate()
    {
        if (isFalling)
        {
            transform.Translate(modifier, 0.0f, 0.0f, Space.World);
            modifier = -modifier;
        }
    }
}
