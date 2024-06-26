using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : Enemy
{
   bool isHidden;
   public float maxStoppedTime;
   float stoppedTimer;

   public float rollingSpeed;
   public bool isRolling;

    bool isAvoidFall;
   protected override void Start()
    {
        base.Start();
        isAvoidFall = autoMovement.avoidFall;
    }

   protected override void  Update()
   {
      base.Update();
      if(isHidden && rb2d.velocity.x == 0f)
      {
         stoppedTimer += Time.deltaTime;
         if(stoppedTimer >=maxStoppedTime)
         {
            ResetMove();
         }
      }
   }
   public override void Stomped(Transform player)
   {
      AudioManager.Instance.PlayStomp();
      isRolling = false; 
      if(!isHidden)
      {
         isHidden = true;
         animator.SetBool("Hidden",isHidden);
         autoMovement.PauseMovement();
      }
      else
      {
         if(Mathf.Abs(rb2d.velocity.x) > 0f)
         {
            autoMovement.PauseMovement();
         }
         else
         {
            if(player.position.x < transform.position.x)
            {
               autoMovement.speed = rollingSpeed;
            }
            else
            {
               autoMovement.speed = -rollingSpeed;
            }
            autoMovement.ContinueMovement(new Vector2(autoMovement.speed, 0f));
            isRolling = true;
         }         
      }
      DestroyOutCamera destroyOutCamera = GetComponent<DestroyOutCamera>();
      if(isRolling)
      {
         destroyOutCamera.onlyBack = false;
         autoMovement.avoidFall = false;
      }
      else
      {
         destroyOutCamera.onlyBack = true;
         autoMovement.avoidFall = isAvoidFall;
      }
        NoDamageTemp();
        stoppedTimer = 0;
   }
    protected void NoDamageTemp()
    {
        gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        Invoke("ResetLayer", 0.1f);
    }
    public override void HitRollingShell()
    {
        if (!isRolling)
        {
            FlipDie();
        }
        else
        {
            autoMovement.ChangeDirection();
        }
    }
   
   void ResetLayer()
   {
      gameObject.layer = LayerMask.NameToLayer("Enemy");
   }
   void ResetMove()
   {
      autoMovement.ContinueMovement();
      isHidden = false;
      animator.SetBool("Hidden", isHidden);
      stoppedTimer = 0;
   }
   protected override void OnCollisionEnter2D (Collision2D collision)
    {
        if (isRolling)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().HitRollingShell();
            }
        }
        else if(!isHidden)
        {
            base.OnCollisionEnter2D(collision);
        }
    }
}
