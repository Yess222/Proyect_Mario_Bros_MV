using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int points;
    protected Animator animator;
    protected AutoMovement autoMovement;
    protected Rigidbody2D rb2d;

    public GameObject floatPointsPrefab;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        autoMovement = GetComponent<AutoMovement>();
        rb2d = GetComponent<Rigidbody2D>();   
    }
    protected virtual void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == gameObject.layer)
        {
            autoMovement.ChangeDirection();
        }
    }
    public virtual void Stomped(Transform player)
    {
        
    }
    public virtual void HitFireball()
    {
        FlipDie();
    }

    public virtual void HitStarman() 
    {
        FlipDie();
    }
    public virtual void HitBelowBlock()
    {
        FlipDie();
    }
    public  virtual void HitRollingShell()
    {
        FlipDie();
    }

    protected void FlipDie()
    {
        AudioManager.Instance.PlayFlipDie();
        animator.SetTrigger("Flip");
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
        if(autoMovement != null)
        {
            autoMovement.enabled = false;
        }
        GetComponent<Collider2D>().enabled = false;
        Dead();
    }
    protected void Dead()
    {
        ScoreManager.Instance.SumarPuntos(points);
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity);
        FloatPoint floatPoints = newFloatPoints.GetComponent<FloatPoint>();
        floatPoints.numPoints = points;

    }
}
