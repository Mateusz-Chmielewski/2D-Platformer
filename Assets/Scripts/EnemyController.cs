using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyController : MonoBehaviour
{

    private float startPosition;
    public float moveRange = 10.0f;
    private Animator animator;
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 1f;
    public float directionValue = 1;

    private bool isMovingRight = true;
    public bool isAlive = true;

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
        FlipTransform(directionValue);
    }

    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        FlipTransform(-directionValue);
    }

    void FlipTransform(float direction)
    {
        gameObject.transform.localScale = FlipXVector(gameObject.transform.localScale, direction); 
    }

    Vector3 FlipXVector(Vector3 vector, float direction)
    {
        return new Vector3(vector.y * direction, vector.y, vector.z);
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
            if (transform.position.y + transform.localScale.y / 3 < other.gameObject.transform.position.y) {
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
             }
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        isAlive = false;
        moveSpeed = 0;
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);    
    }
}

