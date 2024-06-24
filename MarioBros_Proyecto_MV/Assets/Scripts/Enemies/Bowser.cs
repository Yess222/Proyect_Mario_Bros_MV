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

    bool canShot;
    public GameObject firePrefab;
    public Transform shootsPos;
    public float minShowTime = 1f;
    public float maxShowTime = 5f;
    float shotTimer;
    public float minDistanceToShot = 50f;
    float jumpTimer;
    float direction = -1;
    bool canMove;

    public bool collapseBridge;
    protected override void Start()
    {
        base.Start();
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
        shotTimer = Random.Range(minShowTime, maxShowTime);
        canMove = false;
        canShot = false;
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
            
            if(!canShot && Mathf.Abs(Mario.Instance.transform.position.x - transform.position.x) <= minDistanceToShot)
            {
                canShot = true;
            }
            if(canShot)
            {
                shotTimer -= Time.deltaTime;
                if(shotTimer <= 0)
                {
                    Shoot();
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
    void Shoot()
    {
        GameObject fire = Instantiate(firePrefab, shootsPos.position, Quaternion.identity);
        fire.GetComponent<BowserFire>().direction = direction;
        shotTimer  = Random.Range(minShowTime, maxShowTime);
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
