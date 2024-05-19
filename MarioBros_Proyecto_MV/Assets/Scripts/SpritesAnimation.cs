using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public float frameTime = 0.1f;

    //float timer = 0f;
    int animationFrame = 0;

    public bool stop;
    public bool loop = true;

    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Animation();
        StartCoroutine(Animation());
    }

    // Update is called once per frame
    /*void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameTime) 
        {
            //Cambiamos de sprites
            //Sumamos 1 a nuestro indice
            animationFrame++;
            //Tenemos que controlar que el indice no es mayor
            //que el numero de registros del array
            //Si es así, significa que ya hemos pasado el último 
            //y volvemos a empezar
            if (animationFrame >= sprites.Length)
            {
                animationFrame = 0;
            }

            spriteRenderer.sprite = sprites[animationFrame];
            timer = 0;
        }
    }*/

    IEnumerator Animation()
    {
        if (loop)
        {
            //while(animationFrame < sprites.Length)
            while (!stop)
            {
                //Debug.Log("Animation Frame: " + animationFrame);
                spriteRenderer.sprite = sprites[animationFrame];
                //yield return null; //que vuelva en el sgte frame            
                animationFrame++;
                if (animationFrame >= sprites.Length)
                {
                    animationFrame = 0;
                }
                yield return new WaitForSeconds(frameTime);
            }
        }
        else
        {
            while (animationFrame < sprites.Length)
            {
                spriteRenderer.sprite = sprites[animationFrame];
                animationFrame++;
                yield return new WaitForSeconds(frameTime);
            }
            Destroy(gameObject);
        }
        
    }
}
