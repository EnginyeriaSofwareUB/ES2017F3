using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun {


    // Properties //
    public GameObject gameObject { get; private set; }

    public Transform transform {
        get { return gameObject.transform; }
    }

    private Quaternion initAngle;
    //************//

    public Gun(GameObject gameObject) {
        this.gameObject = gameObject;

        var angles = gameObject.transform.rotation.eulerAngles;
        initAngle = Quaternion.Euler(angles.x, angles.y, angles.z);
    }

    public void SetActive(bool activate) {
        gameObject.SetActive(activate);
    }

    public void RestoreGunAngle() {
        gameObject.transform.rotation = initAngle;
    }

}
