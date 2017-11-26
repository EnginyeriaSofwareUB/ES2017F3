using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClock : MonoBehaviour {
   
	GameController control;
    Image clock;
	public Sprite[] sprites;
	public Sprite[] spritesrot;

	private int index;
	private int id;

	private float max;
	private int notrot;

	// Use this for initialization
	void Start () {
		index = 0;
		id = 0;
        clock = GetComponent<Image>();
        control = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();

		notrot = 0;

		max = control.turnTime;
		//spritesrot = (Sprite[])sprites.Clone ();
    }

	// Update is called once per frame
	void Update () {

		for (int i = 0; i <sprites.Length ; i++) {
			if (control.turnRemainingTime> (max*i)/(sprites.Length) && 
				control.turnRemainingTime < (max*(i+1)) / (sprites.Length) ) {

				if (i == sprites.Length-1) {
					//
					float speed = 100F;
					transform.rotation = Quaternion.RotateTowards (
						transform.rotation, Quaternion.Euler (0, 0, notrot * 180), Time.deltaTime * speed);				
				}

				if (i != index) {

					//print (i);

					if (notrot == 0 && index!=2) {
						clock.sprite = sprites [i];
						//print ("AA" + i + " " + notrot +" "+index);

					} else if(notrot==1 && index!=2){
						clock.sprite = spritesrot [i];
						//print ("BB" + i + " " + notrot +" "+index);
					}

					if (i == 0) {
						notrot = (notrot + 1) % 2;
					}
						
					index = (index + 1) % sprites.Length;

				}
			}

		}

		//
        //clock.fillAmount = control.turnRemainingTime / control.turnTime;

	}
}
