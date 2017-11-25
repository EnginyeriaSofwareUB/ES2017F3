using UnityEngine;

public class ArrowProjectile : ExplosiveOnImpact
{
    private void Update()
    {
        if (_isExploding) return;
        // Don't question this, it's 2 AM in the morning and it works, so I'm leaving it as is
        var angle = -Mathf.Rad2Deg * Mathf.Atan2(_rigidbody.velocity.x, _rigidbody.velocity.y);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}