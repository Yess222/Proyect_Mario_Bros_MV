using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    Block block;
    private void Awake()
    {
        block = GetComponentInParent<Block>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HeadMario"))
        {
            collision.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (collision.GetComponentInParent<Mario>().IsBig())
            {
                block.HeadCollision(true);
            }
            else
            {
                block.HeadCollision(false);
            }
            
        }
    }
}
