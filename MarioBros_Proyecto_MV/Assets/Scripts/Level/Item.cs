using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { MagicMushroom, FireFlower, Coin, Life, Star }

public class Item : MonoBehaviour
{
    public ItemType type;
    bool isCatched;

    public Vector2 startVelocity;
    AutoMovement autoMovement;
    private void Awake()
    {
        autoMovement = GetComponent<AutoMovement>();
    }
    private void Start()
    {
       // Invoke("StartMove", 5f);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCatched)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                isCatched = true;
                collision.gameObject.GetComponent<Mario>().CatchItem(type);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<Mario>().CatchItem(type);
            Destroy(gameObject);
        }*/
        if (!isCatched)
        {
            Mario mario = collision.gameObject.GetComponent<Mario>();
            if (mario != null)
            {
                isCatched = true;
                mario.CatchItem(type);
                Destroy(gameObject);
            }
        }   

    }

    public void WaitMove()
    {
        if (autoMovement != null)
        {
            autoMovement.enabled = false;
        }
    }
    public void StartMove()
    {
        if(autoMovement != null)
        {
            autoMovement.enabled = true;
        }
        else
        {
            if(startVelocity != Vector2.zero)
            {
                GetComponent<Rigidbody2D>().velocity = startVelocity;
            }
        }
    }
    public void HitBelowBlock()
    {
        if(autoMovement != null && autoMovement.enabled)
        {
            autoMovement.ChangeDirection();
        }
    }
}
