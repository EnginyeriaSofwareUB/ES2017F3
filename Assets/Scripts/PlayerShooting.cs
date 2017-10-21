using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour {

    private GameObject bulletPrefab, currentGun;
    private Transform bulletSpawn;
    private List<GameObject> guns = new List<GameObject>();

    private float thrust, startPowerTime;
    public float angleSpeed, maxPowerSeconds;
    public int maxPower, minPower, maxAngle;
    private bool shoot;

    public UnityEvent shootEvent;

    void Awake() {

        // Fill all the guns from the model and select empty hands
        for (int i = 0; i < transform.GetChild(0).GetChild(0).GetChild(0).childCount; i++)
        {
            guns.Add(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject);
            Debug.Log("Gun called: '" + transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject.name + "' equipped");
        }

    }

    // Use this for initialization
    void Start()
    {

        // Init variables

        shoot = false;

        currentGun = guns[0];

        if (shootEvent == null)
            shootEvent = new UnityEvent();

    }

    // FixedUpdate call
    void FixedUpdate()
    {

        // If shoot pressed then fire bullet
        if (shoot && gunEquipped())
        {
            Fire();
            // Shoot done
            shoot = false;
        }

    }

    // Update call
    void Update() {

        // If gunEquipped then check inputs related to gun interactions (fire and angle)
        if (gunEquipped()) {
            checkGunInputs();
        }

        checkChangeGun();

    }

    bool gunEquipped()
    {
        return currentGun != guns[0];
    }

    void Fire() {
        // notify all listenrs of this shoot
        if (shootEvent != null) shootEvent.Invoke();


        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add force to the bullet (vector = bulletPos - gunPos)
        var shootingVector = (bullet.transform.position - currentGun.transform.position);
        shootingVector.z = 0;
        bullet.GetComponent<Rigidbody>().AddForce(shootingVector.normalized * thrust, ForceMode.Impulse);      
    }

    void checkGunInputs() {

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

            if (360 - xAngle < maxAngle || xAngle <= (maxAngle + 1))
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


    void checkChangeGun() {

        if (Input.GetKeyDown(KeyCode.Alpha1))
            changeGunTo(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            changeGunTo(1);

    }

    void changeGunTo(int gunIndex) {

        currentGun.SetActive(false);
        currentGun = guns[gunIndex];
        currentGun.SetActive(true);

        // Select spawn and prefab of the gun
        switch (gunIndex) {
            
            // Empty hands
            case 0:
                bulletSpawn = null;
                bulletPrefab = null;
                break;

            // Cannon gun
            case 1:
                bulletSpawn = currentGun.transform.GetChild(0).GetChild(0);
                bulletPrefab = Resources.Load("Prefabs/Bullets/Cannon Ball", typeof(GameObject)) as GameObject;
                break;
        }
    }

}
