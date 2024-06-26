using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPole : MonoBehaviour
{
    public GameObject pointPrefab;
    public Transform flag;
    public Transform bottom;
    public float flagVelocity = 5f;
    public GameObject floatPointsPrefab;
    bool downFlag;
    Mover mover;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mario mario = collision.GetComponent<Mario>();
        if (mario != null)
        {
            downFlag = true;
            mario.Goal();
            mover = collision.GetComponent<Mover>();
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            //Instantiate(pointPrefab, contactPoint, Quaternion.identity);
            CalculateHeight(contactPoint.y);

        }
    }
    private void FixedUpdate()
    {
        if (downFlag)
        {
            if (flag.position.y > bottom.position.y)
            {
                flag.position = new Vector2(flag.position.x, flag.position.y - (flagVelocity * Time.fixedDeltaTime));
            }
            else
            {
                mover.isFlagDown = true;
            }
        }
    }
    void CalculateHeight(float marioPosition)
    {
        float size = GetComponent<BoxCollider2D>().bounds.size.y;
        //Debug.Log("Total size: " + size);
        float minPosition1 = transform.position.y + (size - size / 5f);
        //Debug.Log("Min Position 1: " + minPosition1);
        float minPosition2 = transform.position.y + (size - 2 * size / 5f);
        //Debug.Log("Min Position 2: " + minPosition2);
        float minPosition3 = transform.position.y + (size - 3 * size / 5f);
        //Debug.Log("Min Position 3: " + minPosition3);
        float minPosition4 = transform.position.y + (size - 4 * size / 5f);
        //Debug.Log("Min Position 4: " + minPosition4);

        int numPoints = 0;
        if (marioPosition >= minPosition1)
        {
            numPoints = 5000;
            // ScoreManager.Instance.SumarPuntos(5000);
        }
        else if (marioPosition >= minPosition2)
        {
            numPoints = 2000;
            // ScoreManager.Instance.SumarPuntos(2000);
        }
        else if (marioPosition >= minPosition3)
        {
            numPoints = 800;
            // ScoreManager.Instance.SumarPuntos(800);
        }
        else if (marioPosition >= minPosition4)
        {
            numPoints = 400;
            // ScoreManager.Instance.SumarPuntos(400);
        }
        else
        {
            numPoints = 100;
            // ScoreManager.Instance.SumarPuntos(100);
        }
        ScoreManager.Instance.SumarPuntos(numPoints);

        Vector2 positionFloatPoints = new Vector2(transform.position.x + 0.65f, bottom.position.y);
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, positionFloatPoints, Quaternion.identity);
        FloatPoint floatPoints = newFloatPoints.GetComponent<FloatPoint>();
        floatPoints.numPoints = numPoints;
        floatPoints.speed = flagVelocity;
        floatPoints.distance = flag.position.y - bottom.position.y;
        floatPoints.destroy = false;
    }
}
