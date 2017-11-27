using UnityEngine;

public class ExplosiveOnImpact : ExplosiveBullet
{
    void OnCollisionEnter()
    {
        GetComponent<AudioSource>().Play();
        TriggerExplosion();
    }
}
