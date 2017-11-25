using System.Runtime.InteropServices;
using UnityEngine;

public class Sword : Gun
{
    private Collider _hurtingObjectCollider;
    
    private void Start()
    {
        base.Start();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Hurting Object"))
                _hurtingObjectCollider = child.GetComponent<MeshCollider>();
        }
        DisableDamage();
    }

    public override void Shoot(float thrust, float maxPower)
    {
        anim.SetTrigger("attack");
    }

    public void ActivateDamage()
    {
        _hurtingObjectCollider.enabled = true;
    }

    public void DisableDamage()
    {
        _hurtingObjectCollider.enabled = false;
    }
}