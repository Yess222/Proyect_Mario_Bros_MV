using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected AutoMovement autoMovement;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        autoMovement = GetComponent<AutoMovement>();    
    }
    public virtual void Stomped()
    {

    }
}
