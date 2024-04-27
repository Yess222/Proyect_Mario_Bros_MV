using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected AutoMovement autoMovement;
    protected Rigidbody20 rb20;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        autoMovement = GetComponent<AutoMovement>(); 
        rb2d = GetComponent<RigidBonny2D>();   
    }
    protected virtual void Update()
    {

    }
    public virtual void Stomped()
    {

    }
}
