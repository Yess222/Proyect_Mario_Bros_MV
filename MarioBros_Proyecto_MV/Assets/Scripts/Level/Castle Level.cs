using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleLevel : MonoBehaviour
{
    public string nextScene;
    bool marioInCastle;


    
    void Update()
    {
        if(marioInCastle && LevelManager.Instance.countPoints)
        {
            GameManager.Instance.GoToLevel(nextScene);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Mario mario = collision.gameObject.GetComponent<Mario>();
        if(mario != null)
        {
            mario.transform.position = new Vector3(1000, 1000, 1000);
            marioInCastle = true;
        }
    }
}
