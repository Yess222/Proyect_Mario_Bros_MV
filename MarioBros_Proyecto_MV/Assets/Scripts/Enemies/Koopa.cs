using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : Enemy
{
   bool isHidden;
   public float maxStoppedTime;
   float stoppedTimer;

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
   public override void Stomped()
   {
      if(!isHidden)
      {
         isHidden = true;
         animator.SetBool("Hidden",isHidden);
         autoMovement.PauseMovement();
      }

      gameObject.layer = LayerMask.NameToLayer("OnlyGround");

      Invoke("ResetLayer", 0.1f);
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
}
