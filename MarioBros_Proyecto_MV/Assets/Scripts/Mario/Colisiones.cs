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
    public LayerMask sideColisions;
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

        if (Physics2D.Raycast(footLeft, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
        {
            isGrounded = true;
        }
        else if (Physics2D.Raycast(footRight, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
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
    public bool CheckCollision(int direction)
    {
        Vector2 size = new Vector2(0.1f, col2D.bounds.size.y * 0.8f);
        return Physics2D.OverlapBox(col2D.bounds.center + Vector3.right * direction * col2D.bounds.extents.x,
        col2D.bounds.size, 0, sideColisions);
    }
    private void OnDrawGizmos()
    {
        if (col2D != null)
        {
            Gizmos.DrawWireCube(col2D.bounds.center + Vector3.right * transform.localScale.x * col2D.bounds.extents.x,
            col2D.bounds.size * 0.20f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Lava"))
        {
            if(!mario.isInvincible)
            {
                mario.Hit();
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("DamagePlayer"))
        {
            if (!mario.isInvincible)
            {
                mario.Hit();
            }
        }
    }
    public void Dead()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        foreach (Transform t in transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        }
    }

    public void Respawn()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        foreach (Transform t in transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    public void HurtCollision(bool activate)
    {
        if (activate)
        {
            gameObject.layer = LayerMask.NameToLayer("OnlyGround");
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player");
        }
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
                if (collision.CompareTag("Plant"))
                {
                    mario.Hit();
                }
                else
                {
                    enemy.Stomped(transform);
                    mover.BounceUp();
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Platform") && isGrounded)
        {
            transform.parent = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StompBlock()
    {
        Collider2D collider2D = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (collider2D.gameObject.CompareTag("Block"))
        {
            collider2D.gameObject.GetComponent<Block>().BreakFromTop();
        }
    }

}