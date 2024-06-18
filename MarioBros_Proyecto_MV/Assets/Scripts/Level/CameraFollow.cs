using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followAhead = 2.5f;
    public float minPosX;
    public float maxPosX;

    float camWidth;
    float lastPos;

    void Start()
    {


    }
    void Update()
    {
        float newPosX = target.position.x + followAhead;
        newPosX = Mathf.Clamp(newPosX, minPosX, maxPosX);

        float currentPosX = transform.position.x;
        if (currentPosX < newPosX)
        {
            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);

        }
    }
}
