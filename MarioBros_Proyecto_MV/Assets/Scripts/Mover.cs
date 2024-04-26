using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    enum Direction { Left = -1, None = 0, Right = 1 };
    Direction currentDirection = Direction.None;

    public float speed;
    public float acceleration;
    public float maxVelocity;
    public float friction;
    float currentVelocity = 0f;

    public float jumpForce;
    public float maxJumpingTime = 1f;
    public bool isJumping;
    float jumpTimer = 0;
    float defaultGravity;

    public bool isSkidding;
    public Rigidbody2D rb2D;
    Colisiones colisiones;

    Animaciones animaciones;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultGravity = rb2D.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = colisiones.Grounded();
        animaciones.Grounded(grounded);
        if (isJumping)
        {
            //if(rb2D.velocity.y < 0f)
            //{
            //rb2D.gravityScale = defaultGravity;
            //if (grounded)
            //{
            //isJumping = false;
            //jumpTimer = 0;
            //animaciones.Jumping(false);
            //}
            //}
            if (rb2D.velocity.y > 0f)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    jumpTimer += Time.deltaTime;
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    if (jumpTimer < maxJumpingTime)
                    {
                        rb2D.gravityScale = defaultGravity * 3f;
                    }
                }
            }
            else
            {
                rb2D.gravityScale = defaultGravity;
                if (grounded)
                {
                    isJumping = false;
                    jumpTimer = 0;
                    animaciones.Jumping(false);
                }
            }
        }


        currentDirection = Direction.None;
        //transform.Translate(speed, 0, 0); 

        //if(Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow))
        //{
        //    transform.Translate(0, speed*Time.deltaTime, 0);
        //}
        //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //{
        //    transform.Translate(0, -speed * Time.deltaTime, 0);
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                Jump();
            }
        }


        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.Translate(-speed * Time.deltaTime, 0, 0);
            currentDirection = Direction.Left;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //transform.Translate(speed * Time.deltaTime, 0, 0);
            //MoveRight();
            currentDirection = Direction.Right;
        }

    }
    public void FixedUpdate()
    {
        //Vector2 forceAcceleration = new Vector2((int)currentDirection * acceleration, 0f);
        //rb2D.AddForce(forceAcceleration);
        //float velocityX = Mathf.Clamp(rb2D.velocity.x, -maxVelocity, maxVelocity);


        //Vector2 velocity = new Vector2(velocityX, rb2D.velocity.y);
        //rb2D.velocity = velocity;

        isSkidding = false;
        currentVelocity = rb2D.velocity.x;
        if (currentDirection > 0)
        {
            if (currentVelocity < 0)
            {
                currentVelocity += (acceleration + friction) * Time.deltaTime;
                isSkidding = true;
            }
            else if (currentVelocity < maxVelocity)
            {
                currentVelocity += acceleration * Time.deltaTime;
                transform.localScale = new Vector2(1, 1);
            }
        }
        else if (currentDirection < 0)
        {
            if (currentVelocity > 0)
            {
                currentVelocity -= (acceleration + friction) * Time.deltaTime;
                isSkidding = true;

            }
            else if (currentVelocity > -maxVelocity)
            {
                currentVelocity -= acceleration * Time.deltaTime;
                transform.localScale = new Vector2(-1, 1);
            }
        }
        else
        {
            if (currentVelocity > 1f)
            {
                currentVelocity -= friction * Time.deltaTime;

            }
            else if (currentVelocity < -1f)
            {
                currentVelocity += friction * Time.deltaTime;
            }
            else
            {
                currentVelocity = 0;
            }
        }
        Vector2 velocity = new Vector2(currentVelocity, rb2D.velocity.y);
        rb2D.velocity = velocity;

        animaciones.Velocity(currentVelocity);
        animaciones.Skid(isSkidding);
    }

    void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            Vector2 fuerza = new Vector2(0, jumpForce);
            rb2D.AddForce(fuerza, ForceMode2D.Impulse);
            animaciones.Jumping(true);
        }

    }

    void MoveRight()
    {
        //Vector2 fuerza = new Vector2(10f, 0);
        //rb2D.AddForce(fuerza, ForceMode2D.Impulse);
        Vector2 velocity = new Vector2(1f, 0f);
        rb2D.velocity = velocity;
    }
}