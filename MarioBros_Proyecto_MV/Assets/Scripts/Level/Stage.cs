using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform enterPoint;
    public ConnectDirection enterDirection;

    public CameraFollow cam;
    public bool cameraMove;

    public Color backgroundColor;
    public LevelStageMusic musicBackground;
    void StartStage()
    {
        Mario.Instance.mover.ResetMove();
        LevelManager.Instance.levelPaused = false;
        if (cameraMove)
        {
            cam.StartFollow(Mario.Instance.transform);
        }

    }
    public void EnterStage()
    {
        AudioManager.Instance.PlayLevelStageMusic(musicBackground);
        Camera.main.backgroundColor = backgroundColor;
        Mario.Instance.transform.position = enterPoint.position;
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
        cam.lastPos = transform.position.x;

        switch(enterDirection)
        {
            case ConnectDirection.Down:
                //StartStage();
                StartCoroutine(StartStageDown());
                break;
            case ConnectDirection.Up:
                //StartStage();
                StartCoroutine(StartStageUp());
                break;
            case ConnectDirection.Left:
                StartStage();
                break;
            case ConnectDirection.Right:
                StartStage();
                break;

        }
    }

    IEnumerator StartStageDown()
    {
        yield return new WaitForSeconds(1f);
        StartStage();
    }
    
    IEnumerator StartStageUp()
    {
        float sizeMario = Mario.Instance.GetComponent<SpriteRenderer>().bounds.size.y;
        Mario.Instance.transform.position = enterPoint.position + Vector3.down * sizeMario;
        Mario.Instance.mover.AutoMoveConnection(enterDirection);
        while (!Mario.Instance.mover.moveConnectionCompleted)
        {
            yield return null;
        }
        StartStage();
    }


}
