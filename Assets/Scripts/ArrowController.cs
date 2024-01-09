using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    [Range(1, 10)][SerializeField] private float speed = 5.0f;
    [Range(1, 10)][SerializeField] private float lifeTime = 2.0f;
    private Rigidbody2D rb;
    GameManager GameManager;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemies"))
        {
            GameManager.AddPoints(50);
            GameManager.AddEnemyDestruction();
            Debug.Log("Killed a enemy :)");
            Destroy(gameObject, 0);
        }
        if (other.CompareTag("Blocks"))
        {
            Destroy(gameObject, 0);
        }
        if (other.CompareTag("Boss"))
        {
            Destroy(gameObject, 0);
        }
    }
}
