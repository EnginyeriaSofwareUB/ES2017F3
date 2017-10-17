using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour {

    public GameObject bulletPrefab, bulletGun, gunBase;
    public Transform bulletSpawn;
    private float thrust, startPowerTime;
    public float angleSpeed, maxPowerSeconds;
    public int maxPower, minPower, maxAngle;
    private bool shoot;

    public UnityEvent shootEvent; 

    // Use this for initialization
    void Start()
    {

        // Init variables
        shoot = false;

        if (shootEvent == null)
            shootEvent = new UnityEvent();

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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Time when space pressed
            startPowerTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // Time when space pressed
            thrust = Mathf.Min(maxPower * ((Time.time - startPowerTime) / maxPowerSeconds) + minPower, maxPower);
            shoot = true;
        }


        if (gunBase.name == "Gun Base") {

            // Check rotation of the gun (upwards)
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
            {
                var zAngle = gunBase.transform.localEulerAngles.z;

                if (zAngle % 90 < maxAngle || 360 - zAngle < maxAngle)
                {
                    gunBase.transform.Rotate(0, 0, angleSpeed);
                }
            }

            // Check rotation of the gun (downwards)
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
            {
                var zAngle = gunBase.transform.localEulerAngles.z;

                if (360 - zAngle < maxAngle || zAngle <= (maxAngle + 1))
                {
                    gunBase.transform.Rotate(0, 0, -angleSpeed);
                }
            }
        }

        else if (gunBase.name == "Cannon Base") {
            // Check rotation of the gun (upwards)
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
            {
                var xAngle = gunBase.transform.localEulerAngles.x;

                if (360 - xAngle < maxAngle || xAngle <= (maxAngle + 1))
                {
                    gunBase.transform.Rotate(-angleSpeed, 0, 0);
                }
            }

            // Check rotation of the gun (downwards)
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
            {
                var xAngle = gunBase.transform.localEulerAngles.x;

                if (xAngle % 90 < maxAngle || 360 - xAngle < maxAngle)
                {
                    gunBase.transform.Rotate(angleSpeed, 0, 0);
                }
            }
        }

    }

    void Fire() {
        // notify all listenrs of this shoot
        if (shootEvent != null) shootEvent.Invoke();

        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add force to the bullet (vector = bulletPos - gunPos)
        var shootingVector = (bullet.transform.position - gunBase.transform.position);
        shootingVector.z = 0;
        bullet.GetComponent<Rigidbody>().AddForce(shootingVector.normalized * thrust, ForceMode.Impulse);

        // Destroy the bullet after 2.5 seconds
        Destroy(bullet, 2.5f);
    }
}
