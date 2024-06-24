using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeFinal : MonoBehaviour
{
    public GameObject[] bridgeParts;
    public Transform finalLimit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(CollapseBridge());
            GetComponent<CircleCollider2D>().enabled = false;
            Mario.Instance.mover.StopMove();
            LevelManager.Instance.levelPaused = true;
        }
    }

    IEnumerator CollapseBridge()
    {
        foreach (GameObject bridgePart in bridgeParts)
        {
            Destroy(bridgePart);
            yield return new WaitForSeconds(0.2f);
        }
        //Mario.Instance.mover.ResetMove();
        Mario.Instance.mover.AutoWalk();
        Camera.main.GetComponent<CameraFollow>().UpdateMaxPos(finalLimit.position.x);
    }
}