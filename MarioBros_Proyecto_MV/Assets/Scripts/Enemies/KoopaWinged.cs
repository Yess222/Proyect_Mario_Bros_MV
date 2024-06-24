using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaWinged : Koopa
{
    bool isFly;
    public GameObject wingPrefab;

    protected override void Start()
    {
        base.Start();
        isFly = true;
        animator.SetBool("Fly", true);
    }

    void LoseWings()
    {
        isFly = false;
        if (rb2d.velocity.x < 0)
        {
            autoMovement.speed *= -1;
        }
        rb2d.velocity = Vector2.zero;
        rb2d.isKinematic = false;
        autoMovement.useWaypoints = false;
        autoMovement.isFall = true;
        animator.SetBool("Fly", false);
        LoseWingsAnimation();
    }

    public override void Stomped(Transform player)
    {
        if (isFly)
        {
            AudioManager.Instance.PlayStomp();
            LoseWings();
        }
        else
        {
            base.Stomped(player);  
        }
    }

    protected override void Update()
    {
        base .Update();
        if (Input.GetKeyDown(KeyCode.L))
        {
            Stomped(Mario.Instance.transform);
        }
    }

    void LoseWingsAnimation()
    {
        GameObject wing;
        wing = Instantiate(wingPrefab, transform.position, Quaternion.identity);
        wing.GetComponent<Rigidbody2D>().AddForce(new Vector2(3f, 9f), ForceMode2D.Impulse);

        wing = Instantiate(wingPrefab, transform.position, Quaternion.identity);
        wing.transform.localScale = new Vector3(-1f, 1f, 1f);
        wing.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3f, 9f), ForceMode2D.Impulse);
    }

}
