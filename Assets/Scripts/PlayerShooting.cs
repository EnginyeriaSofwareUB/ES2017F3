using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour
{
    private const string BaseGunPath = "Animator/Model/Character_Hands/";
    private Gun _currentGun;
    public List<Gun> Guns = new List<Gun>();
    private float thrust, startPowerTime;
    public float angleSpeed, maxPowerSeconds;
    public int maxPower, minPower, maxAngle;
    private int initMaxPower, initMinPower, initMaxAngle;
    private bool shoot;

    public UnityEvent shootEvent;

    private void Awake()
    {

        var hands = transform.Find(BaseGunPath);
        // Fill all the guns from the model and select empty hands
        for (var i = 0; i < Guns.Count; i++)
        {
            var instanceGun = Instantiate(Guns[i], hands);
            instanceGun.name = Guns[i].name;
            instanceGun.gameObject.SetActive(false);
            Guns[i] = instanceGun;

            // Adjustments on certain guns
            if (instanceGun.name == "Cannon Base") {
                instanceGun.transform.localPosition = new Vector3(0.2f, -0.2f, -0.5f);
            }

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

        ChangeGunTo(0);

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
        return _currentGun != Guns[0];
    }

    void Fire() {
        // notify all listenrs of this shoot
        if (shootEvent != null) shootEvent.Invoke();

        _currentGun.Shoot(thrust);
        
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
            var xAngle = _currentGun.transform.localEulerAngles.x;

            if ((360 - xAngle < maxAngle || xAngle <= (maxAngle + 1)) && (maxAngle > 0))
            {
                _currentGun.transform.Rotate(-angleSpeed, 0, 0);
            }
        }

        // Check rotation of the gun (downwards)
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
        {
            var xAngle = _currentGun.transform.localEulerAngles.x;

            if (xAngle % 90 < maxAngle || 360 - xAngle < maxAngle)
            {
                _currentGun.transform.Rotate(angleSpeed, 0, 0);
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

    void setCurrentGunActive(bool boolean) {
        if (_currentGun != null) {
            transform.Find(BaseGunPath + _currentGun.name).gameObject.SetActive(boolean);
        }
    }
    
    void ChangeGunTo(int gunIndex) {

        setCurrentGunActive(false);
        _currentGun = Guns[gunIndex];
        setCurrentGunActive(true);

        // Select configurations of the gun
        switch (gunIndex) {
            
            // Empty hands
            case 0:
                RestoreShootingParam();
                break;

            // Cannon gun
            case 1:
                RestoreShootingParam();
                break;

            // Dynamite
            case 2:

                maxAngle = 0;
                maxPower = minPower = 0;
                break;
        }
    }

    void ResetGun() {
        
        // Reset the current gun angle
        //_currentGun.RestoreGunAngle();

        // Set gun to empty hands
        ChangeGunTo(0);
    }

    void RestoreShootingParam() {

        maxAngle = initMaxAngle;
        maxPower = initMaxPower;
        minPower = initMinPower;

    }

}
