using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float direction;
    public float speed;
    public float bounceForce;

    public GameObject explosionPrefab;

    Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        speed *= direction;
        rb2D.velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(0, 0, speed * Time.deltaTime * -45); 
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.HitFireball();
            //Destroy(gameObject);
            Explode(collision.GetContact(0).point);
        }
        else
        {
            Vector2 sidePoint = collision.GetContact(0).normal;
            //Debug.Log("Side point: " + sidePoint);

            if (sidePoint.x != 0) // hay colision lateral
            {
                //Destroy(gameObject);
                Explode(collision.GetContact(0).point);
            }
            else if (sidePoint.y > 0) //colisiona por abajo
            {
                rb2D.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
            else if (sidePoint.y < 0) //colisiona por arriba
            {
                rb2D.AddForce(Vector2.down * bounceForce, ForceMode2D.Impulse);
            }
            else
            {
                //Destroy(gameObject);
                Explode(collision.GetContact(0).point);
            }
        }


        
    }
    void Explode(Vector2 point)
    {
        Instantiate(explosionPrefab, point, Quaternion.identity);
        Destroy(gameObject);
    }
}
