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
        //tell controler that we finished attacking
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>().shoot_ongoing = false;

        Destroy(gameObject);
    }
}
