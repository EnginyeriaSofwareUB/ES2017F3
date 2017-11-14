using UnityEngine;

public class ExplosiveOnImpact : ExplosiveBullet
{
    void OnCollisionEnter()
    {
        TriggerExplosion();
    }
}
