using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followAhead = 2.5f;
    public float minPosX;
    public float maxPosX;

    public Transform limitLeft;
    public Transform limitRight;

    public Transform colLeft;
    public Transform colRight;

    public bool canMove;
    float camWidth;
    float lastPos;

    void Start()
    {
        //float camWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        //Debug.Log("Ancho Camera: " + camWidth);
        //float height = Camera.main.orthographicSize;
        //Debug.Log("Camera.main.aspect: " + Camera.main.aspect);
        //float width = height * Camera.main.aspect;
        //Debug.Log("Otro Ancho: " + width);

        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        minPosX = limitLeft.position.x + camWidth;
        //Debug.Log("Posicion min camara:" + minPosX);
        maxPosX = limitRight.position.x - camWidth;
        //Debug.Log("Posicion max camara:" + maxPosX);
        lastPos = minPosX;

        //colLeft.position = new Vector2(transform.position.x - camWidth - 0.5f, colLeft.position.y);
        //colRight.position = new Vector2(transform.position.x + camWidth + 0.5f, colRight.position.y);
    }
    void Update()
    {
        if(target != null && canMove)
        {
            //transform.position = new Vector3(target.position.x + followAhead, target.position.y, target.position.z);
            float newPosX = target.position.x + followAhead;
            newPosX = Mathf.Clamp(newPosX, lastPos, maxPosX);

            //float currentPosX = transform.position.x;
            //if (currentPosX < newPosX)
            //{
            //    transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
            //}

            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
            lastPos = newPosX;
        }
    }
    public void StartFollow(Transform t)
    {
        target = t;
        float newPosX = target.position.x + followAhead;
        newPosX = Mathf.Clamp(newPosX, lastPos, maxPosX);
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        lastPos = newPosX;
        canMove = true;
    }
    public float PositionInCamera(float pos, float width, out bool limitRight, out bool limitLeft)
    {
        if (pos + width > maxPosX + camWidth)
        {
            limitLeft = false;
            limitRight = true;
            return (maxPosX + camWidth - width);
        }
        else if (pos-width < lastPos-camWidth)
        {
            limitLeft = true;
            limitRight = false;
            return (lastPos - camWidth + width);
        }
        limitLeft = false;
        limitRight = false;
        return pos;
    }
}
