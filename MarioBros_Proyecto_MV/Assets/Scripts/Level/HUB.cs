using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUB : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI numCoins;
    public TextMeshProUGUI time;
    public TextMeshProUGUI worldLevel;

    //int coins;

    // Start is called before the first frame update
    // void Start()
    // {
    //     coins = 0;
    //     numCoins.text = "x" + coins.ToString("D2");
        
    // }

    // Update is called once per frame
    void Update()
    {
        score.text = ScoreManager.Instance.puntos.ToString("D6");
    }
    // public void AddCoins()
    // {
    //     coins++;
    //     numCoins.text = "x" + coins.ToString("D2");
    // }

    public void UpdateCoins(int totalCoins)
    {
        numCoins.text = "x" + totalCoins.ToString("D2");
    }
    public void UpdateTime(float timeLeft)
    {
        int timeLeftInt = Mathf.RoundToInt(timeLeft);
        time.text = timeLeftInt.ToString("D3");

    }

    public void UpdateWorld(int world, int level)
    {
        worldLevel.text = world + "-" + level;
    }
}
