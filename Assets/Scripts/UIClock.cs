using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClock : MonoBehaviour {
    GameController control;
    Image clock;

	// Use this for initialization
	void Start () {
        clock = GetComponent<Image>();
        control = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {

        clock.fillAmount = control.turnRemainingTime / control.turnTime;

	}
}
