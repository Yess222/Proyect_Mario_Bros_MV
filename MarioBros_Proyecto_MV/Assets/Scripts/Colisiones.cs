using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisiones : MonoBehaviour
{
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    BoxCollider2D col2D;
    private void Awake()
    {
        col2D = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Grounded()
    {
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Vector2 footLeft = new Vector2(col2D.bounds.center.x - col2D.bounds.extents.x, col2D.bounds.center.y);
        Vector2 footRight = new Vector2(col2D.bounds.center.x + col2D.bounds.extents.x, col2D.bounds.center.y);


        Debug.DrawRay(footLeft, Vector2.down * col2D.bounds.extents.y * 1.5f, Color.magenta);
        Debug.DrawRay(footRight, Vector2.down * col2D.bounds.extents.y * 1.5f, Color.magenta);

        if (Physics2D.Raycast(footLeft, Vector2.down, col2D.bounds.extents.y * 1.5f, groundLayer))
        {
            isGrounded = true;
        }
        else if (Physics2D.Raycast(footRight, Vector2.down, col2D.bounds.extents.y * 1.5f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        return isGrounded;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Pipe"))
    //    {
    //        Debug.Log("Colision Enter: " + collision.gameObject.name);
    //    }
    //    else if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        Debug.Log("Empezamos a tocar el suelo");
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Pipe")
    //    {
    //        Debug.Log("Colision Stay: " + collision.gameObject.name);
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Pipe"))
    //    {
    //        Debug.Log("Colision Exit: " + collision.gameObject.name);
    //    }
    //    else if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        Debug.Log("Dejamos de tocar el suelo");
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        Debug.Log("Empezamos a tocar el suelo con Trigger");
    //    }
    //}
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //}   
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //        if (collision.gameObject.CompareTag("Ground"))
    //        {
    //            Debug.Log("Dejamos de tocar el suelo con Trigger");
    //        }

    //}
}