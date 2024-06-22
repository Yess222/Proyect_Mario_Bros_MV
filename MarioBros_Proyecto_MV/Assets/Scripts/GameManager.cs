using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public HUB hud;
    int coins;

    public Mario mario;
    
    public int lives;

    bool isRespawning;
    bool isGameOver;

    public bool isLevelCheckPoint;
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //Start is called before the first frame update
    void Start()
    {
        lives = 3;
        coins = 0;
        hud.UpdateCoins(coins);
    }
    public void AddCoins()
    {
        coins++;
        if (coins > 99)
        {
            coins = 0;
            //lives++
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
        if (!isRespawning)
        {
            lives--;
            isRespawning = true;
            if(lives > 0)
            {
                StartCoroutine(Respawn());
            }
            else
            {
                GameOver();
            }
        }
    }
    public void NewLife()
    {
        lives++;
        AudioManager.Instance.PlayOneUp();
    }
    void NewGame()
    {
        lives = 3;
        coins = 0;
        isGameOver = false;
        ScoreManager.Instance.NewGame();
        isLevelCheckPoint = false;
    }
    void GameOver()
    {
        Debug.Log("GameOver");
        isGameOver = true;
        StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        isRespawning = false;
        SceneManager.LoadScene(0);
    }
    public void LevelLoaded()
    {
        if(isGameOver)
        {
            NewGame();
        }
        if (isLevelCheckPoint)
        {
            Mario.Instance.Respawn(LevelManager.Instance.checkPoint.position);
        }
        else
        {
            Mario.Instance.Respawn(LevelManager.Instance.startPoint.position);
        }
        LevelManager.Instance.cameraFollow.StartFollow(Mario.Instance.transform);
    }
    public void KillZone()
    {
        if (!isRespawning)
        {
            AudioManager.Instance.PlayDie();
            LoseLife();
        }
    }

    public void GoToLevel(string sceneName)
    {
        isLevelCheckPoint = false;
        SceneManager.LoadScene(sceneName);
    }

}
