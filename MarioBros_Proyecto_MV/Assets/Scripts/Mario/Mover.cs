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

    public bool inputMoveEnabled = true;

    public GameObject headBox;
    Animaciones animaciones;
    bool isClimbingFlagPole = false;
    public float climbPoleSpeed = 5;
    public bool isFlagDown;

    bool isAutoWalk;
    public float autoWalkSpeed = 5f;
    
    
    Mario mario;

    public bool moveConnectionCompleted = true;

    //CameraFollow cameraFollow;
    SpriteRenderer spriteRenderer;
    

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
        mario = GetComponent<Mario>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultGravity = rb2D.gravityScale;
    }

    void Start()
    {
        //defaultGravity = rb2D.gravityScale;
        //cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    void Update()
    {
        bool grounded = colisiones.Grounded();
        animaciones.Grounded(grounded);

        if (grounded)
        {
            animaciones.Jumping(isJumping);
        }


        if (LevelManager.Instance.levelFinished)
        {
            if (grounded && isClimbingFlagPole)
            {
                StartCoroutine(JumpOffPole());
            }
        }
        else
        {
            headBox.SetActive(false);
            if (isJumping)
            {
                if (rb2D.velocity.y > 0f)
                {
                    headBox.SetActive(true);
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
                    isJumping = false;
                    jumpTimer = 0;
                    //if (grounded)
                    //{
                    //    isJumping = false;
                    //    jumpTimer = 0;
                    //    animaciones.Jumping(false);
                    //}
                }
            }
            currentDirection = Direction.None;
            if (inputMoveEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (grounded)
                    {
                        Jump();
                    }
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    currentDirection = Direction.Left;
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    currentDirection = Direction.Right;
                }
            }
            bool limitRight;
            bool limitLeft;

            if (LevelManager.Instance.cameraFollow != null)
            {
                float posX = LevelManager.Instance.cameraFollow.PositionInCamera(transform.position.x, spriteRenderer.bounds.extents.x,
                out limitRight, out limitLeft);
                if (limitRight && (currentDirection == Direction.Right || currentDirection == Direction.None))
                {
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                }
                else if (limitLeft && (currentDirection == Direction.Left || currentDirection == Direction.None))
                {
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                }
                transform.position = new Vector2(posX, transform.position.y);
            }
        }
        
    }
    public void FixedUpdate()
    {
        //if (LevelManager.Instance.levelFinished)
        //{
            if (isClimbingFlagPole)
            {
                rb2D.MovePosition(rb2D.position + Vector2.down * climbPoleSpeed * Time.fixedDeltaTime);
            }
            else if (isAutoWalk)
            {
                Vector2 velocity = new Vector2(currentVelocity, rb2D.velocity.y);
                rb2D.velocity = velocity;
                animaciones.Velocity(Mathf.Abs(currentVelocity));
            }
        //}
        else if(!rb2D.isKinematic && !LevelManager.Instance.levelFinished)
        {
            //if (!rb2D.isKinematic)
            //{
                isSkidding = false;
                currentVelocity = rb2D.velocity.x;

                if (colisiones.CheckCollision((int)currentDirection))
                {
                    currentVelocity = 0;
                }
                else
                {
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
                }
                if (mario.isCrouched)
                {
                    currentVelocity = 0;
                }

                Vector2 velocity = new Vector2(currentVelocity, rb2D.velocity.y);
                rb2D.velocity = velocity;

                animaciones.Velocity(currentVelocity);
                animaciones.Skid(isSkidding);
           // }
            
        }
    }

    void Jump()
    {
        if (!isJumping)
        {
            if (mario.IsBig())
            {
                AudioManager.Instance.PlayBigJump();
            }
            else
            {
                AudioManager.Instance.PlayJump();
            }
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
    public void Dead(bool bounce)
    {
        inputMoveEnabled = false;
        if (bounce)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.gravityScale = 1;
            rb2D.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }
        
    }
    public void Respawn()
    {
        isAutoWalk = false;
        inputMoveEnabled = true;
        rb2D.velocity = Vector2.zero;
        rb2D.gravityScale = defaultGravity;
        transform.localScale = Vector2.one;
    }
    public void BounceUp()
    {
        rb2D.velocity = Vector2.zero;
        //  Vector2.forceUp = new Vector2(0,10f); Es lo mismo :v
        rb2D.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
    }

    public void AutoWalk()
    {
        rb2D.isKinematic = false;
        inputMoveEnabled = false;
        isAutoWalk = true;
        currentVelocity = autoWalkSpeed;
    }


    public void DownFlagPole()
    {
        if (!isClimbingFlagPole)
        {
            inputMoveEnabled = false;
            rb2D.isKinematic = true;
            rb2D.velocity = new Vector2(0, 0f);
            isClimbingFlagPole = true;
            isJumping = false;
            animaciones.Jumping(false);
            animaciones.Climb(true);
            transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
        }
    }
    IEnumerator JumpOffPole()
    {
        isAutoWalk = false;
        isClimbingFlagPole = false;
        rb2D.velocity = Vector2.zero;
        animaciones.Pause();
        yield return new WaitForSeconds(0.25f);

        while (!isFlagDown)
        {
            yield return null;
        }

        transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
        GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.25f);

        animaciones.Climb(false);
        //rb2D.isKinematic = false;
        animaciones.Continue();
        GetComponent<SpriteRenderer>().flipX = false;
        //isAutoWalk = true;
        //currentVelocity = autoWalkSpeed;
        AutoWalk();
    }

    public void AutoMoveConnection(ConnectDirection direction)
    {
        isAutoWalk = false;
        moveConnectionCompleted = false;
        inputMoveEnabled = false;
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
        spriteRenderer.sortingOrder = -100;

        switch(direction)
        {
            case ConnectDirection.Up:
                //moveConnectionCompleted = true;
                StartCoroutine(AutoMoveConnectionUp());
                break;
            case ConnectDirection.Down:
                //moveConnectionCompleted = true;
                StartCoroutine(AutoMoveConnectionDown());
                break;
            case ConnectDirection.Left:
                moveConnectionCompleted = true;
                break;
            case ConnectDirection.Right:
                //moveConnectionCompleted = true;
                StartCoroutine(AutoMoveConnectionRight());
                break;

        }



    }

    public void ResetMove()
    {
        rb2D.isKinematic = false;
        inputMoveEnabled = true;
        spriteRenderer.sortingOrder = 20;
    }

    IEnumerator AutoMoveConnectionDown()
    {
        float targetDown = transform.position.y - spriteRenderer.bounds.size.y;
        while(transform.position.y > targetDown)
        {
            transform.position += Vector3.down * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        moveConnectionCompleted = true;
    }

    IEnumerator AutoMoveConnectionUp()
    {
        float targetUp = transform.position.y + spriteRenderer.bounds.size.y;
        while (transform.position.y < targetUp)
        {
            transform.position += Vector3.up * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        moveConnectionCompleted = true;
    }

    IEnumerator AutoMoveConnectionRight()
    {
        float targetRight = transform.position.x + spriteRenderer.bounds.size.x*2;
        animaciones.Velocity(1);
        while (transform.position.x < targetRight)
        {
            transform.position += Vector3.right * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        animaciones.Velocity(0);
        moveConnectionCompleted = true;
    }
    public void StopMove()
    {
        inputMoveEnabled = false;
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
        isAutoWalk = false;
        animaciones.Velocity(0);
    }
}