using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();    
    }
    public virtual void Stomped()
    {

    }
}
