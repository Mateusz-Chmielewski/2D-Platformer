using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speed")]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [Range(0.01f, 20.0f)][SerializeField] private float jumpForce = 6f;
    [Space(10)]

    [SerializeField] private GameObject Arrow;
    [SerializeField] private TrailRenderer tr;

    GameManager GameManager;

    const float rayLength = 1f;
    private float dashPower = 24f;
    private bool isDashing = false;
    private float dashTime = 0.2f;
    private float dashCooldown = 1.0f;
    private bool canDash = true;

    private bool hasShooting = false;
    private bool hasDash = false;

    private int score = 0;
    private int arrowCooldown = 0;

    public LayerMask groundLayer;

    private Animator animator;
    private Rigidbody2D rigidBody;

    public int keysFound = 0;
    private int keysNumber = 3;

    private bool isWalking = false;
    private bool doubleJump = true;

    private int lives = 3;

    private Vector2 startPosition;
    private Vector2 mousePosition;
    private Quaternion mouseRotation;

    private bool moveRight = false;
    private bool moveLeft = false;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    public bool fallThrough = false;

    public GameObject temp1;

    void Update()
    {
        fallThrough = false;
        isWalking = false;
        moveRight = false;
        moveLeft = false;
        if (GameManager.currentGameState==GameState.GS_GAME)
        {
            if (IsGrounded())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

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

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                Debug.Log("down pressed");
                fallThrough = true;
            }

            if (moveLeft && moveRight)
            {
                isWalking = false;
            }

            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
            mouseRotation = Quaternion.Euler(0, 0, angle);
            if(arrowCooldown > 0)
            {
                arrowCooldown--;
            }

            if (Input.GetMouseButtonDown(0) && hasShooting)
            {
                if (arrowCooldown == 0)
                {
                    arrowCooldown = 150;
                    Shoot();
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && hasDash)
            {
                Debug.Log("Dash");
                StartCoroutine(Dash());
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
        if (isDashing)
        {
            return;
        }
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

    bool CanJump()
    {
        Debug.Log(rigidBody.velocity.y);
        Debug.Log(coyoteTimeCounter);

        if (IsGrounded() || (coyoteTimeCounter > 0f) && (rigidBody.velocity.y < 0))
        {
            if (IsGrounded())
            {
                Debug.Log("isgroundedjump");
            }
            else
            {
                Debug.Log("isCOYOTEjump");
            }
            coyoteTimeCounter = 0f;
            doubleJump = true;
            return true;
        }
        if (doubleJump)
        {
            doubleJump = false;
            Debug.Log("isdoublejump");
            return true;
        }
        return false;  
    }

    void Jump(bool unconditional)
    {
        if (unconditional || CanJump())
        {
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
            GameManager.AddGem();
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
            if ((transform.position.y > other.gameObject.transform.position.y))
            {
                GameManager.AddPoints(50);
                GameManager.AddEnemyDestruction();
                Debug.Log("Killed a enemy :)");
                Jump(true);
            }
            else
            {
                Debug.Log("Killed a enemy :)");
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
        if (other.CompareTag("KeyShoot"))
        {
            keysFound++;
            GameManager.AddKeys();
            Debug.Log("Klucz elo :)");
            other.gameObject.SetActive(false);
            hasShooting = true;
        }
        if (other.CompareTag("KeyDash"))
        {
            keysFound++;
            GameManager.AddKeys();
            Debug.Log("Klucz elo :)");
            other.gameObject.SetActive(false);
            hasDash = true;
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
        if (other.CompareTag("Hazard"))
        {
            Death();
        }
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
        if (other.CompareTag("Checkpoint"))
        {
            startPosition = transform.position;
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

    private void Shoot()
    {
        Instantiate(Arrow, transform.position, mouseRotation);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0f;
        rigidBody.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        rigidBody.gravityScale = originalGravity;
        rigidBody.velocity = new Vector2(transform.localScale.x * moveSpeed, 0f);
        isDashing = false;
        Debug.Log("done dashing");
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        Debug.Log("can dash");
    }
}

