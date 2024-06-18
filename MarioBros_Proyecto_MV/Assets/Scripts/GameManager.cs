using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HUB hud;
    int coins;
    public Mario mario;
    public int lives;
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        lives = 3;
        coins = 97;
        hud.UpdateCoins(coins);
    }
    public void AddCoins()
    {
        coins++;
        if (coins > 99)
        {
            coins = 0;
            NewLife();
        }
        hud.UpdateCoins(coins);
    }
    public void OutOfTime()
    {
        mario.Dead();
    }
    public void LoseLife()
    {
        lives--;
    }
    public void NewLife()
    {
        lives++;
        AudioManager.Instance.PlayOneUp();
    }
    public void KillZone()
    {
        AudioManager.Instance.PlayDie();
        LoseLife();
    }
}
