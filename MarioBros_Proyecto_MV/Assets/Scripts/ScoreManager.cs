using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int puntos;
    public static ScoreManager Instance;

    private void Awake(){
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        puntos = 0;
    }

    // Update is called once per frame
    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
    }
    
}
