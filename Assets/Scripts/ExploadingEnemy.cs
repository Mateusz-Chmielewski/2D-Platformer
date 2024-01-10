using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploadingEnemy : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool exploaded = false;
    private GameObject explosion;
    public float explosionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        explosion = transform.GetChild(0).gameObject;
        explosion.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("Exploading") && !exploaded)
        {
            explosion.SetActive(true);
            exploaded = true; 
            Debug.Log("Bum");
            spriteRenderer.enabled = false;
            StartCoroutine(KillOnAnimationEnd());
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(explosionTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in zone");
        }
    }
}
