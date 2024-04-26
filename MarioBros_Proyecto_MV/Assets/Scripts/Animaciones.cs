using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaciones : MonoBehaviour
{
    Animator animator;
    //Colisiones colisiones;
    //Mover mover;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //colisiones = GetComponent<Colisiones>();
        //mover = GetComponent<Mover>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
        //animator.SetBool("Grounded", colisiones.Grounded());
        //animator.SetFloat("VelocityX", Mathf.Abs(mover.rb2D.velocity.x));
        //animator.SetBool("Jumping", mover.isJumping);
        //animator.SetBool("Skid", mover.isSkidding);
    //}

    public void Grounded(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);
    }

    public void Velocity(float velocityX)
    {
        animator.SetFloat("VelocityX", Mathf.Abs(velocityX));
    }

    public void Jumping(bool isJumping)
    {
        animator.SetBool("Jumping", isJumping);
    }

    public void Skid(bool isSkidding)
    {
        animator.SetBool("Skid", isSkidding);
    }
}
