using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    public float speed = 1f;
    bool movementPaused;

    Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;

    Vector2 lastVelocity;
    Vector2 currentDirection;
    float defaultSpeed;

    public bool flipSprite = true;
    bool hasBeenVisible;
    public AutoMovement partner;

    Collider2D col2D;
    public bool avoidFall;
    public LayerMask groundLayer;
    public bool isFall;

    public bool useWaypoints;
    public Transform[] waypoints;
    int targetWaypoint = 0;

    float timer = 0;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();
    }
    private void Start()
    {
        //rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        defaultSpeed = Mathf.Abs(speed);
        rb2D.isKinematic = true;
        movementPaused = true;
    }
    public void Activate()
    {
        hasBeenVisible = true;
        if (!useWaypoints)
        {
            rb2D.isKinematic = false;
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        }        
        movementPaused = false;
        if (partner != null)
        {
            partner.Activate();
        }
    }
    private void Update()
    {
        if (spriteRenderer.isVisible && !hasBeenVisible)
        {
            Activate();
        }
    }
    private void FixedUpdate()
    {
        if (!movementPaused)
        {
            //if (rb2D.velocity.x > -0.1f && rb2D.velocity.x < 0.1f)
            //{
            //    if (timer > 0.05f)
            //    {
            //        speed = -speed;
            //    }
            //    timer += Time.deltaTime;
            //}
            //else
            //{
            //    timer = 0;
            //}
            //rb2D.velocity = new Vector2(speed, rb2D.velocity.y);

            if (useWaypoints)
            {
                Vector3 direction = transform.position - waypoints[targetWaypoint].position;
                rb2D.velocity = speed * direction.normalized;
                if(Vector2.Distance(transform.position, waypoints[targetWaypoint].position) < 0.1f)
                {
                    targetWaypoint++;
                    if(targetWaypoint >= waypoints.Length)
                    {
                        targetWaypoint = 0;
                    }
                }
            }
            else
            {
                if (isFall)
                {
                    if (CheckGrounded())
                    {
                        isFall = false;
                    }
                }
                else
                {
                    if (CheckSideCollision())
                    {
                        ChangeDirection();
                    }
                    else if (avoidFall && !CheckGrounded())
                    {
                        ChangeDirection();
                    }
                    else
                    {
                        CheckTimeStopped();
                    }
                    rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
                }
            }            

            if (flipSprite)
            {
                if (rb2D.velocity.x > 0)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }

        }
    }
    public void PauseMovement()
    {
        if (!movementPaused)
        {
            if (!useWaypoints)
            {
                currentDirection = rb2D.velocity.normalized;
                lastVelocity = rb2D.velocity;
            }
            
            movementPaused = true;
            rb2D.velocity = new Vector2(0, 0);
        }
    }
    public void ContinueMovement()
    {
        if (movementPaused)
        {
            if (!useWaypoints)
            {
                speed = defaultSpeed * currentDirection.x;
                rb2D.velocity = new Vector2(speed, lastVelocity.y);
            }
            
            movementPaused = false;
        }
    }
    public void ContinueMovement(Vector2 newVelocity)
    {
        if (movementPaused)
        {
            if (!useWaypoints)
            {
                rb2D.velocity = newVelocity;                
            }
            movementPaused = false;
        }
    }
    public void ChangeDirection()
    {
        speed = -speed;
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        timer = 0;
    }

    bool CheckGrounded()
    {
        if (isFall)
        {
            Vector2 center = new Vector2(col2D.bounds.center.x, col2D.bounds.center.y);
            if(Physics2D.Raycast(center, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (speed > 0)
        {
            Vector2 footRight = new Vector2(col2D.bounds.center.x + col2D.bounds.extents.x, col2D.bounds.center.y);
            if(Physics2D.Raycast(footRight, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (speed < 0)
        {
            Vector2 footLeft = new Vector2(col2D.bounds.center.x - col2D.bounds.extents.x, col2D.bounds.center.y);
            if (Physics2D.Raycast(footLeft, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }

    bool CheckSideCollision()
    {
        Vector3 direction = Vector3.right * speed;
        return Physics2D.OverlapBox(col2D.bounds.center + direction.normalized * col2D.bounds.extents.x,
            col2D.bounds.size * 0.2f, 0, groundLayer);
    }

    void CheckTimeStopped()
    {
        if (Mathf.Abs(rb2D.velocity.x) < 0.1f)
        {
            if (timer > 0.05f)
            {
                ChangeDirection();
            }
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
    }










}
