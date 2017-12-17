using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{

    public Transform SpawnPoint;
    public GameObject BulletPrefab;
    protected Animator anim;
    private float thrust;
    public float minShootSpeed;

    // Positive integer for limited use of variables, negative for infinite use
    public int InitialUsagesLeft;

    public bool InfiniteUses
    {
        get { return InitialUsagesLeft < 0; }
    }

    public UnityEvent bulletFired;

    // Use this for initialization
    internal void Start()
    {
        anim = GetComponent<Animator>();
        //print(anim!=null);
        if (bulletFired == null)
            bulletFired = new UnityEvent();

    }

    public virtual void Shoot(float thrust, float maxPower)
    {
        this.thrust = thrust;
        print(anim != null);
        if (anim != null)
        {
            anim.SetTrigger("shoot");
            if (maxPower > 0) {
                var speed = (thrust / maxPower) + minShootSpeed;
                anim.speed = speed;
            }
        }
        else
        {
            ShootBullet();
        }
    }

    public void ShootBullet()
    {
        

        print("Shoot bullet");
        var shootingVector = SpawnPoint.position - transform.position;
        shootingVector.z = 0;
        var projectedPosition = SpawnPoint.position;
        projectedPosition.z = -0.88f;
        var bullet = Instantiate(
            BulletPrefab,
            projectedPosition,
            Quaternion.Euler(0, 0, 0));

        Camera.main.GetComponent<FollowingCamera>().bullet_target = bullet;
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>().shoot_ongoing = true;

        // Add force to the bullet (vector = bulletPos - gunPos)
        bullet.GetComponent<Rigidbody>().AddForce(shootingVector.normalized * thrust, ForceMode.Impulse);

        if (bulletFired != null) bulletFired.Invoke();

    }
}
