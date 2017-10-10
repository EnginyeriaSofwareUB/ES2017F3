using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public GameObject bulletPrefab, bulletGun, gunBase;
    public Transform bulletSpawn;
    private float thrust, startPowerTime, angleSpeed, maxPowerSeconds;
    private int maxPower, maxAngle, minPower;
    private bool shoot;

    // Use this for initialization
    void Start()
    {

        // Init variables
        maxPower = 40;
        maxAngle = 50;
        minPower = 3;
        angleSpeed = 0.6f;
        maxPowerSeconds = 3.5f;
        shoot = false;

    }

    // FixedUpdate call
    void FixedUpdate()
    {

        // If shoot pressed then fire bullet
        if (shoot)
        {
            Fire();
            // Shoot done
            shoot = false;
        }

    }



    // Update call
    void Update()
    {


        // Check fire
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Time when space pressed
            startPowerTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Time when space pressed
            thrust = Mathf.Min(maxPower * ((Time.time - startPowerTime) / maxPowerSeconds) + minPower, maxPower);
            shoot = true;
        }

        // Check rotation of the gun (upwards)
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow))
        {
            var zAngle = gunBase.transform.localEulerAngles.z;

            if (zAngle % 90 < maxAngle || 360 - zAngle < maxAngle)
            {
                gunBase.transform.Rotate(0, 0, angleSpeed);
            }
        }

        // Check rotation of the gun (downwards)
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            var zAngle = gunBase.transform.localEulerAngles.z;

            if (360 - zAngle < maxAngle || zAngle <= (maxAngle + 1))
            {
                gunBase.transform.Rotate(0, 0, -angleSpeed);
            }
        }

    }

    void Fire()
    {

        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add force to the bullet (vector = bulletPos - gunPos)
        var shootingVector = (bullet.transform.position - bulletGun.transform.position);
        shootingVector.z = 0;
        bullet.GetComponent<Rigidbody>().AddForce(shootingVector.normalized * thrust, ForceMode.Impulse);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 5.0f);
    }
}
