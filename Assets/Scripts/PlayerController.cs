using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speed")]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [Range(0.01f, 20.0f)][SerializeField] private float jumpForce = 6f;
    [Space(10)]

    GameManager GameManager;

    const float rayLength = 2f;

    private int score = 0;

    public LayerMask groundLayer;

    private Animator animator;
    private Rigidbody2D rigidBody;

    private int keysFound = 0;
    private int keysNumber = 3;

    private bool isWalking = false;
    private bool doubleJump = true;

    private Vector3 theScale;

    private int lives = 3;

    private Vector2 startPosition;

    private bool moveRight = false;
    private bool moveLeft = false;

    public GameObject temp1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isWalking = false;
        moveRight = false;
        moveLeft = false;
        if (GameManager.currentGameState==GameState.GS_GAME)
        {
            Debug.Log("GRAMY");
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                moveRight = true;
                isWalking = true;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                moveLeft = true;
                isWalking = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump(false);
            }
            animator.SetBool("isGrounded", IsGrounded());
            animator.SetBool("isWalking", isWalking);
            //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);
        }
        else
        {
            Debug.Log(GameManager.currentGameState);
        }
    }

    private void FixedUpdate()
    {
        if(moveRight == true)
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        if (moveLeft == true)
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Awake()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    bool IsGrounded()
    {
        Vector2 leftGroundRayPos = new Vector2(transform.position.x - 0.5f, transform.position.y);
        Vector2 rightGroundRayPos = new Vector2(transform.position.x + 0.5f, transform.position.y);
        Debug.DrawRay(leftGroundRayPos, Vector2.down);
        Debug.DrawRay(rightGroundRayPos, Vector2.down);
        return (Physics2D.Raycast(rightGroundRayPos, Vector2.down, rayLength, groundLayer.value) || (Physics2D.Raycast(leftGroundRayPos, Vector2.down, rayLength, groundLayer.value)));
    }

    void Jump(bool unconditional)
    {
        if (IsGrounded() || doubleJump == true || unconditional)
        {
            if (!IsGrounded())
            {
                doubleJump = false;
            }
            else
            {
                doubleJump = true;
            }
            rigidBody.velocity = Vector2.zero;
            rigidBody.angularVelocity = 0f;
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            //Debug.Log("Jumper");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bonus"))
        {
            GameManager.AddPoints(10);
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
        }
        if(other.CompareTag("Finish"))
        {
            if(keysFound<keysNumber)
            {
                Debug.Log("Szukaj kluczy");
            }
            else
            {
                GameManager.LevelCompletion();
                Debug.Log("WIN");
            }
        }
        if (other.CompareTag("Enemies"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            bool isAlive = enemy.isAlive;
            if ((transform.position.y>other.gameObject.transform.position.y)&&(isAlive))
            {
                GameManager.AddPoints(50);
                GameManager.AddEnemyDestruction();
                Debug.Log("Killed a enemy :)");
                Jump(true);
            }
            else if(isAlive)
            {
                GameManager.UpdateHearth();
                Death();
            }
        }
        if (other.CompareTag("Key"))
        {
            keysFound++;
            GameManager.AddKeys();
            Debug.Log("Klucz elo :)");
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Heart"))
        {
            lives++;
            Debug.Log("zycie plus :)");
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("FallLevel"))
        {
            Death();
        }
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent( null );
        }
    }

    private void Death()
    {
        transform.position = startPosition;
        lives--;
        if (lives < 0)
        {
            Debug.Log("GAME OVER");
        }
        else
        {
            Debug.Log("Lives remaining " + lives);
        }
    }
}

