using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    private const int NumberOfCastResults = 5;

    CapsuleCollider2D touchingCollider;
    Animator animator;
    public ContactFilter2D contactFilter;
    public float groundDistance = 0.1f;
    public float wallDistance = 0.05f;
    public float cellingDistance = 0.05f;

    RaycastHit2D[] groundHits = new RaycastHit2D[NumberOfCastResults];
    RaycastHit2D[] wallHits = new RaycastHit2D[NumberOfCastResults];
    RaycastHit2D[] cellingHits = new RaycastHit2D[NumberOfCastResults];

    public bool IsGrounded { get; private set; }
    public bool IsOnWall { get; private set; }
    public bool IsOnCelling { get; private set; }

    private Vector2 WallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;


    private void Awake()
    {
        touchingCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = touchingCollider.Cast(Vector2.down, contactFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCollider.Cast(WallCheckDirection, contactFilter, wallHits, wallDistance) > 0;
        IsOnCelling = touchingCollider.Cast(Vector2.up, contactFilter, cellingHits, cellingDistance) > 0;
    }
}
