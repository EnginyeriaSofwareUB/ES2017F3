using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public GameObject bulletPrefab, bulletGun, gunBase;
    public Transform bulletSpawn;
    private float thrust, startPowerTime;

	// Use this for initialization
	void Start () {

	}

    void Update () {

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


    // FixedUpdate call
    void FixedUpdate()
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
            thrust = Mathf.Min(30*((Time.time - startPowerTime)/5) + 3, 30);
            Fire();
        }

        // Check rotation of the gun (upwards)
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow))
        {
            var zAngle = gunBase.transform.localEulerAngles.z;

            if (zAngle % 90 < 50 || 360 - zAngle < 50)
            {
                gunBase.transform.Rotate(0, 0, 1);
            }
        }

        // Check rotation of the gun (downwards)
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            var zAngle = gunBase.transform.localEulerAngles.z;

            if (360 - zAngle < 50 || zAngle <= 51)
            {
                gunBase.transform.Rotate(0, 0, -1);
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
