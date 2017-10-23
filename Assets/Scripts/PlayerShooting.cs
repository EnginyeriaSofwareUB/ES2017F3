using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour {

    private Gun currentGun;
    private GameObject bulletPrefab;
    private Transform bulletSpawn;
    private List<Gun> guns = new List<Gun>();

    private float thrust, startPowerTime;
    public float angleSpeed, maxPowerSeconds;
    public int maxPower, minPower, maxAngle;
    private int initMaxPower, initMinPower, initMaxAngle;
    private bool shoot;

    public UnityEvent shootEvent;

    void Awake() {

        // Fill all the guns from the model and select empty hands
        for (int i = 0; i < transform.GetChild(0).GetChild(0).GetChild(0).childCount; i++)
        {
            guns.Add(new Gun(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject));
            Debug.Log("Gun called: '" + transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject.name + "' equipped");
        }

    }

    // Use this for initialization
    void Start()
    {

        // Init variables
        initMaxPower = maxPower;
        initMinPower = minPower;
        initMaxAngle = maxAngle;

        shoot = false;

        currentGun = guns[0];

        if (shootEvent == null)
            shootEvent = new UnityEvent();

    }

    // FixedUpdate call
    void FixedUpdate()
    {

        // If shoot pressed then fire bullet
        if (shoot && GunEquipped())
        {
            Fire();
            // Shoot done
            shoot = false;
        }

    }

    // Update call
    void Update() {

        // If gun equipped then check inputs related to gun interactions (fire and angle)
        if (GunEquipped()) {
            CheckGunInputs();
        }

        CheckChangeGun();

    }

    bool GunEquipped() {
        return currentGun != guns[0];
    }

    void Fire() {
        // notify all listenrs of this shoot
        if (shootEvent != null) shootEvent.Invoke();

        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            Quaternion.Euler(0, 0, bulletSpawn.rotation.z));

        // Add force to the bullet (vector = bulletPos - gunPos)
        var shootingVector = (bullet.transform.position - currentGun.transform.position);
        shootingVector.z = 0;
        bullet.GetComponent<Rigidbody>().AddForce(shootingVector.normalized * thrust, ForceMode.Impulse);

        // Reset to empty hands and reset angle of the gun fired
        ResetGun();

    }


    void CheckGunInputs() {

        //////////// Check fire /////////////
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



        ///////////// Check rotation of the gun /////////////////
        // Upwards rotation
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
        {
            var xAngle = currentGun.transform.localEulerAngles.x;

            if ((360 - xAngle < maxAngle || xAngle <= (maxAngle + 1)) && (maxAngle > 0))
            {
                currentGun.transform.Rotate(-angleSpeed, 0, 0);
            }
        }

        // Check rotation of the gun (downwards)
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
        {
            var xAngle = currentGun.transform.localEulerAngles.x;

            if (xAngle % 90 < maxAngle || 360 - xAngle < maxAngle)
            {
                currentGun.transform.Rotate(angleSpeed, 0, 0);
            }
        }
        
    }


    void CheckChangeGun() {

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeGunTo(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeGunTo(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeGunTo(2);

    }

    void ChangeGunTo(int gunIndex) {

        currentGun.SetActive(false);
        currentGun = guns[gunIndex];
        currentGun.SetActive(true);

        // Select configurations of the gun
        switch (gunIndex) {
            
            // Empty hands
            case 0:

                RestoreShootingParam();

                bulletSpawn = null;
                bulletPrefab = null;
                break;

            // Cannon gun
            case 1:

                RestoreShootingParam();

                bulletSpawn = currentGun.transform.GetChild(0).GetChild(0);
                bulletPrefab = Resources.Load("Prefabs/Bullets/Cannon Ball", typeof(GameObject)) as GameObject;

                break;

            // Dynamite
            case 2:

                maxAngle = 0;
                maxPower = minPower = 0;

                bulletSpawn = currentGun.transform.GetChild(0);
                bulletPrefab = Resources.Load("Prefabs/Bullets/Dynamite", typeof(GameObject)) as GameObject;
                break;
        }
    }

    void ResetGun() {
        
        // Reset the current gun angle
        currentGun.RestoreGunAngle();

        // Set gun to empty hands
        ChangeGunTo(0);
    }

    void RestoreShootingParam() {

        maxAngle = initMaxAngle;
        maxPower = initMaxPower;
        minPower = initMinPower;

    }

}
