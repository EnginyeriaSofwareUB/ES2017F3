using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public GameObject bulletPrefab, bulletGun, gunBase;
    public Transform bulletSpawn;
    private float thrust, startPowerTime, angleSpeed, maxPowerSeconds;
    private int maxPower, maxAngle, minPower;

    // Use this for initialization
    void Start()
    {

        // Init variables
        maxPower = 40;
        maxAngle = 50;
        minPower = 3;
        angleSpeed = 0.6f;
        maxPowerSeconds = 3.5f;

    }

    //// FixedUpdate call
    //void FixedUpdate () {


    //       // Check fire
    //       if (Input.GetKeyDown(KeyCode.Space))
    //       {

    //           // Perform the fire
    //           Fire();
    //       }

    //       // Check rotation of the gun (upwards)
    //       if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow))
    //       {
    //           var zAngle = gunBase.transform.localEulerAngles.z;

    //           if ((!negativeAngle && zAngle < 50) || negativeAngle)
    //           {
    //               gunBase.transform.Rotate(0, 0, 1);

    //               if (negativeAngle && zAngle >= 359)
    //                   negativeAngle = false;
    //           }
    //       }

    //       // Check rotation of the gun (downwards)
    //       if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow))
    //       {
    //           var zAngle = gunBase.transform.localEulerAngles.z;

    //           //print(zAngle + "   " + negativeAngle);

    //           if ((negativeAngle && (zAngle > 310 || zAngle < 0)) || !negativeAngle)
    //           {
    //               gunBase.transform.Rotate(0, 0, -1);

    //               if (!negativeAngle && zAngle < 1 && zAngle >= 0)
    //                   negativeAngle = true;
    //           }
    //       }

    //   }


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
            Fire();
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
        bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.position - bulletGun.transform.position) * thrust, ForceMode.Impulse);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}
