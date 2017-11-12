﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{

    public Transform SpawnPoint;
    public GameObject BulletPrefab;
    private Animator anim;
    private float thrust;
    public float minShootSpeed;

    public UnityEvent bulletFired;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        if (bulletFired == null)
            bulletFired = new UnityEvent();

    }

    public void Shoot(float thrust, float maxPower)
    {
        this.thrust = thrust;

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
        var bullet = (GameObject)Instantiate(
            BulletPrefab,
            SpawnPoint.position,
            Quaternion.Euler(0, 0, SpawnPoint.rotation.z));

        // Add force to the bullet (vector = bulletPos - gunPos)
        var shootingVector = bullet.transform.position - transform.position;
        shootingVector.z = 0;

        bullet.GetComponent<Rigidbody>().AddForce(shootingVector.normalized * thrust, ForceMode.Impulse);

        if (bulletFired != null) bulletFired.Invoke();

    }
}