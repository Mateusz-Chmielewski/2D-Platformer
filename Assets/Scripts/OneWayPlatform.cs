using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    PlayerController player;
    PlatformEffector2D platformEffector;

    void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<PlayerController>();

        }

    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<PlayerController>();

        }

    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        if (player.fallThrough)
        {
            platformEffector.surfaceArc = 0;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        platformEffector.surfaceArc = 180;
        player = null;
    }
}
