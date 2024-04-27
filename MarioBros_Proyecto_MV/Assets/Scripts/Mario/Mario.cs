using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    enum State { Default=0,Super=1}
    State currentState = State.Default;
    public GameObject stompBox;

    Mover mover;
    Colisiones colisiones;
    Animaciones animaciones;
    Rigidbody2D rb2D;

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

        if(Input.GetKeyDown(KeyCode.P))
        {
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
    }
}
