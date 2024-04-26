using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    public float speed = 1f;
    bool movementPaused;

    Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
    }
    private void FixedUpdate()
    {
        if (!movementPaused)
        {
            if(rb2D.velocity.x > -0.1f && rb2D.velocity.x < -0.1f)
            {
                speed=-speed;
            }
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);

            if(rb2D.velocity.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }
    public void PauseMovement()
    {
        if (!movementPaused)
        {
            movementPaused = true;
            rb2D.velocity = new Vector2(0,0);
        }
    }
}
