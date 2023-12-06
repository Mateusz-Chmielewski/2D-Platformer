using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyController : MonoBehaviour
{

    private float startPosition;
    public float moveRange = 10.0f;
    private Animator animator;
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 1f;

    private bool isMovingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.x > startPosition + moveRange)
        {
            isMovingRight = false;
        }
        if (this.transform.position.x < startPosition - moveRange)
        {
            isMovingRight = true;
        }
        if(isMovingRight)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
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

    void Awake()
    {
        animator = GetComponent<Animator>();
        startPosition = (this.transform.position.x);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (transform.position.y < other.gameObject.transform.position.y) {
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
             }
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        moveSpeed = 0;
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);    
    }
}

