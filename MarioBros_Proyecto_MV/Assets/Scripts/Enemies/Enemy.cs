using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected AutoMovement autoMovement;
    protected Rigidbody2D rb2d;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        autoMovement = GetComponent<AutoMovement>(); 
        rb2d = GetComponent<Rigidbody2D>();   
    }
    protected virtual void Update()
    {

    }
    public virtual void Stomped(Transform player)
    {
        
    }

    public virtual void HitFireball()
    {
        FlipDie();
    }

    void FlipDie()
    {
        animator.SetTrigger("Flip");
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
        if(autoMovement != null)
        {
            autoMovement.enabled = false;
        }
        GetComponent<Collider2D>().enabled = false;
    }
}
