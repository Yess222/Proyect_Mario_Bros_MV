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
    
    //CameraFollow cameraFollow;
    Mario mario;
    SpriteRenderer spriteRenderer;
    

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
        mario = GetComponent<Mario>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        defaultGravity = rb2D.gravityScale;
        //cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    void Update()
    {
        bool grounded = colisiones.Grounded();
        animaciones.Grounded(grounded);
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
                    if (grounded)
                    {
                        isJumping = false;
                        jumpTimer = 0;
                        animaciones.Jumping(false);
                    }
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
        }
        bool limitRight;
        bool limitLeft;
        
        if(LevelManager.Instance.cameraFollow != null)
        {
            float posX = LevelManager.Instance.cameraFollow.PositionInCamera(transform.position.x, spriteRenderer.bounds.extents.x,
            out limitRight, out limitLeft);
            if (limitRight && (currentDirection == Direction.Right || currentDirection == Direction.None)) {
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }else if (limitLeft && (currentDirection == Direction.Left || currentDirection == Direction.None)){
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }
            transform.position = new Vector2(posX, transform.position.y);
        }
    }
    public void FixedUpdate()
    {
        if (LevelManager.Instance.levelFinished)
        {
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
        }
        else
        {
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
    public void Dead()
    {
        inputMoveEnabled = false;
        rb2D.velocity = Vector2.zero;
        rb2D.gravityScale = 1;
        rb2D.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
    }
    public void Respawn()
    {
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

    public void DownFlagPole()
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
    IEnumerator JumpOffPole()
    {
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
        rb2D.isKinematic = false;
        animaciones.Continue();
        GetComponent<SpriteRenderer>().flipX = false;
        isAutoWalk = true;
        currentVelocity = autoWalkSpeed;
    }
}