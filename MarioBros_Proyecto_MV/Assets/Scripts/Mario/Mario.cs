using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    enum State { Default = 0, Super = 1, Fire = 2 }
    State currentState = State.Default;
    public GameObject stompBox;

    Mover mover;
    Colisiones colisiones;
    Animaciones animaciones;
    Rigidbody2D rb2D;

    public GameObject fireBallPrefab;
    public Transform shootPos;

    public bool isInvincible;
    public float invincibleTime;
    float invincibleTimer;


    public bool isHurt;
    public float hurtTime;
    float hurtTimer;

    public bool isCrouched;


    //public GameObject headBox;
    public bool levelFinished;
    bool isDead;
    private void Awake()
    {
        mover = GetComponent<Mover>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        isCrouched = false;
        if (!isDead)
        {
            if (rb2D.velocity.y < 0)
            {
                stompBox.SetActive(true);
            }
            else
            {
                stompBox.SetActive(false);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (colisiones.Grounded())
                {
                    isCrouched = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (isCrouched && currentState != State.Default)
                {
                    colisiones.StompBlock();
                }
                else
                {
                    Shoot();
                }

            }
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer <= 0)
                {
                    isInvincible = false;
                    animaciones.InvincibleMode(false);
                }
            }
            if (isHurt)
            {
                hurtTimer -= Time.deltaTime;
                if (hurtTimer <= 0)
                {
                    EndHurt();
                }
            }
        }

        animaciones.Crouch(isCrouched);


        //if(rb2D.velocity.y > 0 && !isDead)
        //{
        //    headBox.SetActive(true);
        //}
        //else
        //{
        //    headBox.SetActive(false);
        //}

        //if(Input.GetKeyDown(KeyCode.P))
        //{   
        //   Time.timeScale = 0;
        //    animaciones.PowerUp();
        //}
        //if(Input.GetKeyDown(KeyCode.H))
        //{
        //    Hit();
        //}
    }
    public void Hit()
    {
        if (!isHurt)
        {
            if (currentState == State.Default)
            {
                Dead();
            }
            else
            {
                Time.timeScale = 0;
                animaciones.Hit();
                StartHurt();
            }
        }
    }

    void StartHurt()
    {
        isHurt = true;
        animaciones.Hurt(true);
        hurtTimer = hurtTime;
        colisiones.HurtCollision(true);
    }

    void EndHurt()
    {
        isHurt = false;
        animaciones.Hurt(false);
        colisiones.HurtCollision(false);
    }
    public void Dead()
    {
        if (!isDead)
        {
            isDead = true;
            colisiones.Dead();
            mover.Dead();
            animaciones.Dead();
        }

    }
    void ChangeState(int newState)
    {
        currentState = (State)newState;
        animaciones.NewState(newState);
        Time.timeScale = 1;
    }
    public void CatchItem(ItemType type)
    {


        switch (type)
        {
            case ItemType.MagicMushroom:
                //MagicMushroom
                if (currentState == State.Default)
                {
                    animaciones.PowerUp();
                    Time.timeScale = 0;
                }
                break;
            case ItemType.FireFlower:
                //FireFlower
                if (currentState != State.Fire)
                {
                    animaciones.PowerUp();
                    Time.timeScale = 0;
                }
                break;
            case ItemType.Coin:
                //Coin
                Debug.Log("Coin");
                break;
            case ItemType.Life:
                //Life
                break;
            case ItemType.Star:
                //Star
                isInvincible = true;
                animaciones.InvincibleMode(true);
                invincibleTimer = invincibleTime;
                EndHurt();
                break;
        }

    }
    void Shoot()
    {
        if (currentState == State.Fire && !isCrouched)
        {
            GameObject newFireBall = Instantiate(fireBallPrefab, shootPos.position, Quaternion.identity);
            newFireBall.GetComponent<Fireball>().direction = transform.localScale.x;
            animaciones.Shoot();
        }
    }
    public bool IsBig()
    {
        return currentState != State.Default;
    }
    public void Goal()
    {
        mover.DownFlagPole();
        levelFinished = true;
    }
}
