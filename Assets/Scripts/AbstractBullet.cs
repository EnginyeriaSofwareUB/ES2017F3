using UnityEngine;

public abstract class AbstractBullet : MonoBehaviour
{
    public float BulletDamage = 10f;
    public float TimeUntilDespawn = 10f;

    public void Start()
    {
        Invoke("DespawnBullet", TimeUntilDespawn);
    }

    protected abstract void DespawnBullet();
}
