using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public HUB hub;
    int coins;

    public int time;
    public float timer;
    Mario mario;
    public bool levelFinished;
    public static LevelManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        coins = 0;
        hub.UpdateCoins(coins);
        timer = time;
        hub.UpdateTime(timer);
        mario = FindAnyObjectByType<Mario>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!levelFinished)
        {
            timer -= Time.deltaTime /0.4f; //1 segundo del juego equivale a 0.4s. reales
        
            if(timer <= 0)
            {
                //Mario mario = FindAnyObjectByType<Mario>();
                // mario.Dead();
                timer = 0;
            }
            hub.UpdateTime(timer);
        }
        
    }
    public void AddCoins()
    {
        coins++;
        hub.UpdateCoins(coins);
    }
    public void LevelFinished()
    {
        levelFinished = true;
        StartCoroutine(SecondsToPoints());
    }
    IEnumerator SecondsToPoints()
    {
        yield return new WaitForSeconds(1f);

        int timeLeft = Mathf.RoundToInt(timer);

        while(timeLeft > 0)
        {
            timeLeft--;
            hub.UpdateTime(timeLeft);
            ScoreManager.Instance.SumarPuntos(50);
            AudioManager.Instance.PlayCoin();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
