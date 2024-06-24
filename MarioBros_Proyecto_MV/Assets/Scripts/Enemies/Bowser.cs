using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowser : Enemy
{
    public int health = 5;
    public bool isDead;
    public float speed = 2f;
    public float minJumpTime = 1f;
    public float maxJumpTime = 5f;
    public float jumpForce = 8f;
    public float minDistanceToMove = 10f;

    float jumpTimer;
    float direction = -1;
    bool canMove;

    public bool collapseBridge;
    protected override void Start()
    {
        base.Start();
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
        canMove = false;
    }
    protected override void Update()
    {
        if(!collapseBridge)
        {
            if(!canMove && Mathf.Abs(Mario.Instance.transform.position.x - transform.position.x) <= minDistanceToMove)
            {
                canMove = true;
            }
            if(canMove)
            {
                if(transform.position.x >= (Mario.Instance.transform.position.x + 2f) && direction == 1)
                {
                    direction = -1;
                    transform.localScale = Vector3.one;
                }
                else if(transform.position.x <= (Mario.Instance.transform.position.x - 2f) && direction  == -1)
                {
                    direction = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                rb2d.velocity = new Vector2(direction*speed, rb2d.velocity.y);

                jumpTimer -= Time.deltaTime;
                if(jumpTimer <= 0)
                {
                    Jump();
                }
            }
        }
    }
    void Jump()
    {
        Vector2 force = new Vector2(0, jumpForce);
        rb2d.AddForce(force, ForceMode2D.Impulse);
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);

    }

    public void FallBridge()
    {
        Dead();
    }

    public override void Stomped(Transform player)
    {
        player.GetComponent<Mario>().Hit();
    }
    public override void HitRollingShell()
    {
        
    }
    public override void HitBelowBlock()
    {
    
    }
    public override void HitFireball()
    {
        rb2d.velocity = Vector2.zero;
        health--;
        if(health <= 0)
        {
            FlipDie();
            isDead = true;
            // Dead();
            // Debug.Log("Muerto");
        }
    }
    public override void HitStarman()
    {

    }
}   
