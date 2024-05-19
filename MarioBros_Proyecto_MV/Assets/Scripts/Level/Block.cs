using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isBreakable;
    public GameObject brickPiecePrefab;

    public int numCoins;
    public GameObject coinBlockPrefab;
    
    bool bouncing;

    public Sprite emptyBlock;
    bool isEmpty;

    public GameObject itemPreab;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HeadMario"))
        {
            collision.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (isBreakable)
            {
                Break();
            }
            else if (!isEmpty)
            {
                if (numCoins > 0)
                {
                    if (!bouncing)
                    {
                        Instantiate(coinBlockPrefab, transform.position, Quaternion.identity);
                        numCoins--;
                        Bounce();
                        if (numCoins <= 0)
                        {
                            isEmpty = true;
                        }
                    }
                }
                else if(itemPreab != null)
                {
                    if (!bouncing)
                    {
                        StartCoroutine(ShowItem());
                        Bounce();
                        isEmpty = true;
                    }
                }
            }
            
            //Debug.Log("Head Mario");
            //Bounce();
            //Break();
        }
        
    }

    void Bounce()
    {
        if(!bouncing)
        {
            StartCoroutine(BounceAnimation());
        }
    }
    IEnumerator BounceAnimation()
    {
        bouncing = true;
        float time = 0;
        float duration = 0.1f;

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.25f;

        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;

        }
        transform.position = targetPosition;
        time = 0;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(targetPosition, startPosition, time / duration);
            time += Time.deltaTime;
            yield return null;

        }
        transform.position = startPosition;
        bouncing = false;
        if (isEmpty)
        {
            SpritesAnimation spritesAnimation = GetComponent<SpritesAnimation>();
            if(spritesAnimation != null)
            {
                spritesAnimation.stop = true;
            }
            GetComponent<SpriteRenderer>().sprite = emptyBlock;
        }
    }
    void Break()
    {
        GameObject brickPiece;
        //Arriba a la derecha
        brickPiece = Instantiate(brickPiecePrefab, transform.position,
            Quaternion.Euler(new Vector3(0, 0, 0)));
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(6f, 12f);

        //arriba a la izquierda
        brickPiece = Instantiate(brickPiecePrefab, transform.position,
            Quaternion.Euler(new Vector3(0, 0, 90)));
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(-6f, 12f);

        //abajo a la derecha
        brickPiece = Instantiate(brickPiecePrefab, transform.position,
            Quaternion.Euler(new Vector3(0, 0, -90)));
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(6f, -8f);

        //abajo a la izquierda
        brickPiece = Instantiate(brickPiecePrefab, transform.position,
            Quaternion.Euler(new Vector3(0, 0, 180)));
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(-6f, -8f);

        Destroy(gameObject);
    }

    IEnumerator ShowItem()
    {
        GameObject newItem = Instantiate(itemPreab, transform.position, Quaternion.identity);

        AutoMovement autoMovement = newItem.GetComponent<AutoMovement>();
        if(autoMovement != null )
        {
            autoMovement.enabled = false;
        }
        float time = 0;
        float duration = 1f;
        Vector2 startPosition = newItem.transform.position;
        Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.5f;

        while(time < duration )
        {
            newItem.transform.position = Vector2.Lerp(startPosition, targetPosition, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        newItem.transform.position = targetPosition;
        if(autoMovement != null)
        {
            autoMovement.enabled = true;
        }
    }
}
