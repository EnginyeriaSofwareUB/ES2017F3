using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour
{
    private const string ModelCenter = "Animator/Model_Center/", BaseGunPath = ModelCenter + "Model/Character_Hands/", ModelHands = ModelCenter + "Model/Character_Base/";
    private Transform hands, modelCenter, mdlHandLeft, mdlHandRight;
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
        modelCenter = transform.Find(ModelCenter);
        mdlHandLeft = transform.Find(ModelHands+"Hand_Left_001");
        mdlHandRight = transform.Find(ModelHands + "Hand_right_001");

        // Fill all the guns from the model and select empty hands
        for (var i = 0; i < _guns.Count; i++)
        {
            var instanceGun = Instantiate(_guns[i], hands);
            instanceGun.name = _guns[i].name;
            instanceGun.gameObject.SetActive(false);
            _guns[i] = instanceGun;

            // Attach listener to bulletFired
            instanceGun.bulletFired.AddListener(EmptyHands);

            // Dynamic adjustments on each gun (position and scale)
            Vector3 newPos, newScale;

            switch (instanceGun.name) {
                case "Cannon Base":
                    newScale = Vector3.one * 0.4f;
                    newPos = new Vector3(0.78f, 0, -0.15f);
                    break;

                case "Dynamite Base":
                    newScale = Vector3.one * 1.9f;
                    newPos = new Vector3(-0.019f, 0.426f, -0.448f);
                    break;
                    
                default:
                    newScale = newPos = Vector3.one;
                    break;
            }

            instanceGun.transform.localScale = newScale;
            instanceGun.transform.localPosition = newPos;


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

        EmptyHands();

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
        return _currentGun != null;
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
            // Time when fire pressed
            startPowerTime = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // Time when fire released
            thrust = Mathf.Min(maxPower * ((Time.time - startPowerTime) / maxPowerSeconds) + minPower, maxPower);
            shoot = true;
        }

        ///////////// Check rotation of the gun /////////////////
        /// (only when gun aiming is possible)
        if (maxAngle > 0) {
            if ( (hands.localScale.z > 0 && modelCenter.localScale.z > 0) || (hands.localScale.z > 0 && modelCenter.localScale.z < 0) )
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
            if ((360 - rotAngle + angleSpeed < maxAngle || rotAngle + angleSpeed <= (maxAngle - angleSpeed)) && (maxAngle > 0))
            {
                hands.Rotate(0, 0, angleSpeed);
            }
        }

        // Check rotation of the gun (downwards)
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
        {
            if ((rotAngle - angleSpeed < maxAngle) || (360 - rotAngle + angleSpeed <= (maxAngle - angleSpeed) && rotAngle >= 360 - maxAngle))
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
            if (360 - rotAngle - angleSpeed < maxAngle || rotAngle + angleSpeed <= (maxAngle - angleSpeed))
            {
                hands.Rotate(0, 0, angleSpeed);
            }
        }

        // Check rotation of the gun (downwards)
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
        {
            if ((rotAngle - angleSpeed < maxAngle) || (360 - rotAngle + angleSpeed <= (maxAngle - angleSpeed) && rotAngle >= 360 - maxAngle))
            {
                hands.Rotate(0, 0, -angleSpeed);
            }
        }
    }


    void CheckChangeGun() {

        if (Input.GetKeyDown(KeyCode.Alpha1))
            EmptyHands();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeGunTo(0);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeGunTo(1);

    }

    void setCurrentGunActive(bool boolean) {
        if (_currentGun != null) {
            transform.Find(BaseGunPath + _currentGun.name).gameObject.SetActive(boolean);
        }
    }

    public void EmptyHands() {
        //Set gun to empty hands (animated hands)
        setCurrentGunActive(false);
        _currentGun = null;
        SetHandsAnimation(true);
    }

    void SetHandsAnimation(bool boolean) {
        mdlHandRight.gameObject.SetActive(boolean);
        mdlHandLeft.gameObject.SetActive(boolean);
    }

    void ChangeGunTo(int gunIndex) {

        // Change gun only when changing to another one
        if (_currentGun == _guns[gunIndex]) return;

        if (!GunEquipped()) SetHandsAnimation(false);

        setCurrentGunActive(false);
        _currentGun = _guns[gunIndex];
        setCurrentGunActive(true);

        // Select configurations of the gun
        switch (gunIndex) {
            
            // Cannon gun
            case 0:
                RestoreShootingParam();
                break;

            // Dynamite
            case 1:

                maxAngle = 0;
                maxPower = minPower = 0;
                break;
        }

        // If facing left then reverse hands
        if (modelCenter.localScale.z < 0) {
            if (hands.localScale.z > 0) ReverseHands();
        }
        // If facing right after facing left reverse hands
        else {
            if (hands.localScale.z < 0) ReverseHands();
        }

        // Reset hands angle
        hands.rotation = Quaternion.Euler(0,0,0);

    }

    void ReverseHands() {
        var scale = hands.localScale;

        scale.x *= -1;
        scale.z *= -1;

        hands.localScale = scale;
    }

    void RestoreShootingParam() {

        maxAngle = initMaxAngle;
        maxPower = initMaxPower;
        minPower = initMinPower;

    }

}
