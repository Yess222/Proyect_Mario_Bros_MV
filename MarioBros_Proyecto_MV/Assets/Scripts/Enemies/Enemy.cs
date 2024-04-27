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
}
