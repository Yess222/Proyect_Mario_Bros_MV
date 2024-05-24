using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{
    public override void HitFireball()
    {
        Destroy(transform.parent.gameObject);
    }
    public override void HitStarman()
    {
        Destroy(transform.parent.gameObject);
    }
    public override void HitRollingShell()
    {
        Destroy(transform.parent.gameObject);
    }
}
