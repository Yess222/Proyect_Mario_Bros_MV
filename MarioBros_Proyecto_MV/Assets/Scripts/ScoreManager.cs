using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int puntos;
    public int maxPuntos;
    public static ScoreManager Instance;
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
    void Start()
    {
        puntos = 0;
        // if(PlayerPrefs.HasKey("Puntos"))
        // {
        //     maxPuntos = PlayerPrefs.GetInt("Puntos");
        // }
        // else
        // {
        //     maxPuntos = 0;
        //     PlayersPrefs.SetInt("Puntos", maxPuntos);
        // }
        maxPuntos = PlayerPrefs.GetInt("Puntos", 0);

    }
    public void NewGame()
    {
        puntos = 0;
    }
    public void GameOver()
    {
        if(puntos > maxPuntos)
        {
            maxPuntos = puntos;
            // PlayerPrefs.SetInt("Puntos", maxPuntos);
            PlayerPrefs.SetInt("Puntos", maxPuntos);
        }
    }
    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
    }

}
