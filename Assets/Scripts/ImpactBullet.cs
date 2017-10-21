using UnityEngine;

public class ImpactBullet : AbstractBullet
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerController>().Damage(BulletDamage);
        if (other.gameObject.CompareTag("DestructibleCube"))
            Destroy(other.gameObject);
        DespawnBullet();

    }

    protected override void DespawnBullet()
    {
        Destroy(gameObject);
    }
}