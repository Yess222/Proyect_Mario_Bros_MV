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

    float timer = 0;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        rb2D.isKinematic = false;
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
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
            if (rb2D.velocity.x > -0.1f && rb2D.velocity.x < 0.1f)
            {
                if (timer > 0.05f)
                {
                    speed = -speed;
                }
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
            }
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);

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
            currentDirection = rb2D.velocity.normalized;
            lastVelocity = rb2D.velocity;
            movementPaused = true;
            rb2D.velocity = new Vector2(0, 0);
        }
    }
    public void ContinueMovement()
    {
        if (movementPaused)
        {
            speed = defaultSpeed * currentDirection.x;
            rb2D.velocity = new Vector2(speed, lastVelocity.y);
            movementPaused = false;
        }
    }
    public void ContinueMovement(Vector2 newVelocity)
    {
        if (movementPaused)
        {
            rb2D.velocity = newVelocity;
            movementPaused = false;
        }
    }
    public void ChangeDirection()
    {
        speed = -speed;
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
    }
}
