using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public World[] worlds;

    public int currentWorld;
    public int currentLevel;

    public HUB hud;
    int coins;

    public Mario mario;
    
    public int lives;

    bool isRespawning;
    public bool isGameOver;

    //public bool isLevelCheckPoint;
    public int currentPoint;
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
        // lives = 3;
        // coins = 0;
        // hud.UpdateCoins(coins);
        HideTimer();
        isGameOver = true;
        currentWorld = PlayerPrefs.GetInt("World",1);
        currentLevel = PlayerPrefs.GetInt("Level",1);
    }
    // private void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.R))
    //     {
    //         StartGame();
    //     }
    //     if(Input.GetKeyDown(KeyCode.C))
    //     {
    //         ContinueGame();
    //     }
    // }
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
        //mario.Dead();
        Mario.Instance.Dead(true);
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
    public void StartGame()
    {
        currentLevel = 1;
        currentWorld = 1;
        LoadLevel();
    }
    public void ContinueGame()
    {
        LoadLevel();
    }
    void NewGame()
    {
        lives = 3;
        coins = 0;
        isGameOver = false;
        ScoreManager.Instance.NewGame();
        //isLevelCheckPoint = false;
        currentPoint = 0;
    }
    void GameOver()
    {
        Debug.Log("GameOver");
        ScoreManager.Instance.GameOver();
        isGameOver = true;
        // currentLevel = 1;
        // currentWorld = 1;
        PlayerPrefs.SetInt("World", currentWorld);
        PlayerPrefs.SetInt("Level", currentLevel);
        StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);
        isRespawning = false;
        //SceneManager.LoadScene(0);
        //LoadTransition();
        SceneManager.LoadScene("Transition");
        if(isGameOver)
        {
            AudioManager.Instance.PlayGameOver();
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("StartMenu");
        }
        else
        {   
            yield return new WaitForSeconds(5f);
            LoadLevel();
        }
    }
    public void LevelLoaded()
    {
        hud.UpdateWorld(currentWorld, currentLevel);
        ShowTimer();
        if(isGameOver)
        {
            NewGame();
        }
        hud.UpdateCoins(coins);

        Vector3 position = LevelManager.Instance.checkPoints[currentPoint].startPointPlayer.position;
        Mario.Instance.Respawn(position);
        LevelManager.Instance.StartLevel(currentPoint);
        //if (isLevelCheckPoint)
        //{
        //    Mario.Instance.Respawn(LevelManager.Instance.checkPoint.position);
        //}
        //else
        //{
        //    Mario.Instance.Respawn(LevelManager.Instance.startPoint.position);
        //}
        LevelManager.Instance.cameraFollow.StartFollow(Mario.Instance.transform);
    }
    public void KillZone()
    {
        if (!isRespawning)
        {
            //AudioManager.Instance.PlayDie();
            //LoseLife();
            Mario.Instance.Dead(false);
        }
    }

    public void GoToLevel(string sceneName)
    {
        //isLevelCheckPoint = false;
        currentPoint = 0;
        SceneManager.LoadScene(sceneName);
    }

    public void GoToLevel(int world, int level)
    {
        //isLevelCheckPoint = false;
        currentPoint = 0;
        currentLevel = level;
        currentWorld = world;
        hud.UpdateWorld(world, level);
        LoadTransition(); 
    }
    void LoadTransition()
    {
        SceneManager.LoadScene("Transition");
        Invoke("LoadLevel", 5f);
    }

    void LoadLevel()
    {
        //foreach(World w in worlds)
        //{
        //    if (w.id == currentWorld)
        //    {
        //        foreach(Level l in w.levels)
        //        {
        //            if(l.id == currentLevel)
        //            {
        //                SceneManager.LoadScene(l.sceneName);
        //                return;
        //            }
        //        }
        //    }
        //}

        int worldIndex = currentWorld - 1;
        int levelIndex = currentLevel - 1;

        string sceneName = worlds[worldIndex].levels[levelIndex].sceneName;
        SceneManager.LoadScene(sceneName);

    }

    public void NextLevel()
    {
        
        int worldIndex = currentWorld - 1;
        int levelIndex = currentLevel - 1;

        levelIndex++;
        if(levelIndex >= worlds[worldIndex].levels.Length)
        {
            worldIndex++;
            if(worldIndex >= worlds.Length)
            {
                Debug.Log("Juego Terminado");
                return;
            }
            else
            {
                levelIndex = 0;
            }
        }
        currentWorld = worldIndex + 1;
        currentLevel = levelIndex + 1;
        //isLevelCheckPoint = false;
        currentPoint = 0;
        hud.UpdateWorld(currentWorld, currentLevel);
        LoadTransition() ;
    }

    public void HideTimer()
    {
        hud.time.enabled = false;
    }
    public void ShowTimer()
    {
        hud.time.enabled = true;
    }
}
[System.Serializable]
public struct World
{
    public int id;
    public Level[] levels;
}

[System.Serializable]
public struct Level
{
    public int id;
    public string sceneName;

}
