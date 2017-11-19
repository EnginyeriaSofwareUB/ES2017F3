using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour
{
    private const string BaseGunPath = "Animator/Model_Center/Model/Character_Hands/", Model = "Animator/Model_Center/Model/";
    private Transform hands, model;
    private Gun _currentGun;
    private List<Gun> _guns;
    private float thrust, startPowerTime;
    public float angleSpeed, maxPowerSeconds;
    public int maxPower, minPower, maxAngle;
    private int initMaxPower, initMinPower, initMaxAngle;
    private bool shoot;

    public UnityEvent shootEvent;

    private void Awake()
    {
        _guns = new List<Gun>(GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>().AvailableGuns);
        hands = transform.Find(BaseGunPath);
        model = transform.Find(Model);
        
        // Fill all the guns from the model and select empty hands
        for (var i = 0; i < _guns.Count; i++)
        {
            var instanceGun = Instantiate(_guns[i], hands);
            instanceGun.name = _guns[i].name;
            instanceGun.gameObject.SetActive(false);
            _guns[i] = instanceGun;

            // Attach listener to bulletFired
            instanceGun.bulletFired.AddListener(ResetGun);

            // Adjustments on certain guns
            if (instanceGun.name == "Cannon Base") {
                instanceGun.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                instanceGun.transform.localPosition = new Vector3(0.4f, 0, 0);
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
        return _currentGun != _guns[0];
    }

    void Fire() {
        // notify all listenrs of this shoot
        if (shootEvent != null) shootEvent.Invoke();

        _currentGun.Shoot(thrust, maxPower);

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
        /// (only when gun aiming is possible)
        if (maxAngle > 0) {
            if (model.localScale.x > 0)
            {
                CheckPositiveRotation(hands.eulerAngles.z);
            }
            else
            {
                CheckNegativeRotation(hands.eulerAngles.z);
            }
        }
        
    }

    void CheckPositiveRotation(float rotAngle) {

        // Upwards rotation
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
        {
            if ((360 - rotAngle + angleSpeed < maxAngle || rotAngle + angleSpeed <= (maxAngle - 0.2f)) && (maxAngle > 0))
            {
                hands.Rotate(0, 0, angleSpeed);
            }
        }

        // Check rotation of the gun (downwards)
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
        {
            if ((rotAngle - angleSpeed < maxAngle) || (360 - rotAngle + angleSpeed <= (maxAngle - 0.2f) && rotAngle >= 360 - maxAngle))
            {
                hands.Rotate(0, 0, -angleSpeed);
            }
        }
    }

    void CheckNegativeRotation(float rotAngle) {

        // Scale < 0 => rotations inverted
        // Upwards rotation
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
        {
            if (360 - rotAngle - angleSpeed < maxAngle || rotAngle + angleSpeed <= (maxAngle - 0.2f))
            {
                hands.Rotate(0, 0, -angleSpeed);
            }
        }

        // Check rotation of the gun (downwards)
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
        {
            if ((rotAngle - angleSpeed < maxAngle) || (360 - rotAngle + angleSpeed <= (maxAngle - 0.2f) && rotAngle >= 360 - maxAngle))
            {
                hands.Rotate(0, 0, angleSpeed);
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

        // Change gun only when changing to another one
        if (_currentGun == _guns[gunIndex]) return;

        setCurrentGunActive(false);
        _currentGun = _guns[gunIndex];
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

        // Reset hands angle
        hands.rotation = Quaternion.Euler(0,0,0);

    }

    void ResetGun() {
        // Set gun to empty hands
        ChangeGunTo(0);
    }

    void RestoreShootingParam() {

        maxAngle = initMaxAngle;
        maxPower = initMaxPower;
        minPower = initMinPower;

    }

}
