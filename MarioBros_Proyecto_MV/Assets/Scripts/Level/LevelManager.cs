using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // public HUB hub;
    // int coins;

    public int time;
    public float timer;

    Mario mario;

    public bool levelFinished;
    public bool levelPaused;

    //public Transform startPoint;
    //public Transform checkPoint;
    public CheckPoint[] checkPoints;
    public bool hasLevelStart;

    public CameraFollow cameraFollow;

    public bool countPoints;

    public LevelStageMusic musicBackground;

    public static LevelManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // coins = 0;
        // hub.UpdateCoins(coins);
        timer = time;
        GameManager.Instance.hud.UpdateTime(timer);

        mario = FindAnyObjectByType<Mario>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        GameManager.Instance.LevelLoaded();
    }

    public void StartLevel(int currentPoint)
    {
        if(hasLevelStart && currentPoint == 0)
        {
            levelPaused = true;
            Mario.Instance.mover.AutoWalk();
        }
        if (checkPoints[currentPoint].stage != null)
        {
            checkPoints[currentPoint].stage.EnterStage();
        }else
        {
            AudioManager.Instance.PlayLevelStageMusic(musicBackground);            
        }

        Camera.main.backgroundColor = checkPoints[currentPoint].backgroundColor;

    }

    public void MarioInCastle()
    {
        cameraFollow.canMove = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!levelFinished && !levelPaused)
        {
            timer -= Time.deltaTime / 0.4f; //1 segundo del juego equivale a 0.4s. reales
            if (timer <= 100)
            {
                AudioManager.Instance.HurryUp();
            }
            if (timer <= 0)
            {
                //Mario mario = FindAnyObjectByType<Mario>();
                // mario.Dead();
                GameManager.Instance.OutOfTime();
                timer = 0;
            }
            GameManager.Instance.hud.UpdateTime(timer);
        }
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

        while (timeLeft > 0)
        {
            timeLeft--;
            GameManager.Instance.hud.UpdateTime(timeLeft);
            ScoreManager.Instance.SumarPuntos(50);
            AudioManager.Instance.PlayCoin();
            yield return new WaitForSeconds(0.05f);
        }
        countPoints = true;
    }
}
