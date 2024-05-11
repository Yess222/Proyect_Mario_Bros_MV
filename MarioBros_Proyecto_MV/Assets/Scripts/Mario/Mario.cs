using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    enum State { Default=0,Super=1,Fire=2}
    State currentState = State.Default;
    public GameObject stompBox;

    Mover mover;
    Colisiones colisiones;
    Animaciones animaciones;
    Rigidbody2D rb2D;

    public GameObject headBox;
    bool isDead;
    private void Awake(){
        mover = GetComponent<Mover>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (rb2D.velocity.y < 0 && !isDead)
        {
            stompBox.SetActive(true);
        }
        else
        {
            stompBox.SetActive(false);
        }

        if(rb2D.velocity.y > 0)
        {
            headBox.SetActive(true);
        }
        else
        {
            headBox.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {   
            Time.timeScale = 0;
            animaciones.PowerUp();
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            Hit();
        }
    }
    public void Hit()
    {
        if(currentState == State.Default)
        {
            Dead();
        }
        else
        {
            Time.timeScale =0;
            animaciones.Hit();
        }
        
    }
    public void Dead()
    {
        if(!isDead)
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
        /*if (type == ItemType.MagicMushroom)
        {
            //MagicMushroom
        }
        else if (type == ItemType.FireFlower) 
        {
            //FireFlower
        }
        else if (type == ItemType.Coin)
        {
            //Coin
        }
        else if (type == ItemType.Life)
        {
            //Life
        }
        else if (type == ItemType.Star)
        {
            //Star
        }*/

        switch (type)
        {
            case ItemType.MagicMushroom:
                //MagicMushroom
                if(currentState == State.Default)
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
                break;            
        }

    }

}
