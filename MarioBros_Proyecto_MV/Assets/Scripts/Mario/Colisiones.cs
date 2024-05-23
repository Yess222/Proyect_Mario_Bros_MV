using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisiones : MonoBehaviour
{
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    BoxCollider2D col2D;
    Mario mario;
    Mover mover;

    private void Awake()
    {
        col2D = GetComponent<BoxCollider2D>();
        mario = GetComponent<Mario>();
        mover = GetComponent<Mover>();
    }

    public bool Grounded()
    {
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Vector2 footLeft = new Vector2(col2D.bounds.center.x - col2D.bounds.extents.x, col2D.bounds.center.y);
        Vector2 footRight = new Vector2(col2D.bounds.center.x + col2D.bounds.extents.x, col2D.bounds.center.y);


        Debug.DrawRay(footLeft, Vector2.down * col2D.bounds.extents.y * 1.5f, Color.magenta);
        Debug.DrawRay(footRight, Vector2.down * col2D.bounds.extents.y * 1.5f, Color.magenta);

        if (Physics2D.Raycast(footLeft, Vector2.down, col2D.bounds.extents.y * 1.5f, groundLayer))
        {
            isGrounded = true;
        }
        else if (Physics2D.Raycast(footRight, Vector2.down, col2D.bounds.extents.y * 1.5f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        return isGrounded;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (mario.isInvincible)
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.HitStarman();
            }
            else
            {
                mario.Hit();
            }
    
        }
    }
    public void Dead()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDead");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // PlayerHit playerHit = collision.GetComponent<PlayerHit>();
        // if (playerHit != null)
        // {
        //     playerHit.Hit();
        //     mover.BounceUp();
        // }
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (mario.isInvincible)
            {
                enemy.HitStarman();
            }
            else
            {
                enemy.Stomped(transform);
                mover.BounceUp();
            }
        }
    }

}