using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutCamera : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool hasBeenVisible;
    public bool onlyBack;
    public float minDistance = 0;
    public GameObject parent;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    public void Update()

    {
        if (spriteRenderer.isVisible)
        {
            hasBeenVisible = true;
        }
        else
        {
            if (hasBeenVisible)
            {
                if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > minDistance)
                {
                    if (onlyBack)
                    {
                        if (transform.position.x > Camera.main.transform.position.x)
                        {
                            return;
                        }
                    }
                    if (parent == null)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(parent);
                    }
                }


            }
        }

    }
}
