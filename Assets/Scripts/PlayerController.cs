using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speed")]
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f;
    [Range(0.01f, 20.0f)] [SerializeField] private float jumpForce = 6f;
    [Space(10)]

    const float rayLength = 2f;

    public LayerMask groundLayer;

    private Rigidbody2D rigidBody;
    private bool doubleJump = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        } 
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);
    }

    void Awake()  
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);

    }

    void Jump()
    {
        if (IsGrounded() || doubleJump == true)
        {
            if (!IsGrounded())
            {
                doubleJump = false;
            } else
            {

                doubleJump = true;
            }
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            Debug.Log("Jumper");
        }
    }
}
