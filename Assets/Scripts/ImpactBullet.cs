using UnityEngine;

public class ImpactBullet : AbstractBullet
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerController>().Damage(BulletDamage);
        DespawnBullet();
    }

    protected override void DespawnBullet()
    {
        Destroy(gameObject);
    }
}
