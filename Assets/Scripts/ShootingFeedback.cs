﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingFeedback : MonoBehaviour {
    GameController control;
    public Image UI_bar;
    public Text UI_text;
    //public Slider UI_bar_slider;

    [Space(5)]
    public float barVelocity = 1.75f;
    public float time = 0f;
    float maxShootTime;

    public Color[] bar_colors;


    float thrust;
    float maxPower;

	// Use this for initialization
	void Start () {
        control = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();
        maxShootTime = GetComponent<PlayerShooting>().maxPowerSeconds;
        maxPower = GetComponent<PlayerShooting>().maxPower;

        UI_bar = control.UI_shoot_bar.GetComponent<Image>();
        UI_text = control.UI_shoot_text.GetComponent<Text>();

        //UI_bar = GameObject.FindGameObjectWithTag("ShootUI_bar").GetComponent<Image>();
        //UI_text = GameObject.FindGameObjectWithTag("ShootUI_text").GetComponent<Text>();

        UI_bar.rectTransform.sizeDelta = new Vector2(1f, UI_bar.rectTransform.sizeDelta.y);
    }
	
	// Update is called once per frame
	void Update () {
        if (control.activePlayer.Equals(this.gameObject) && control.current != GameController.gameStates.gameOver) //Si som el actiu y no es gameover..
        {
            UI_bar.gameObject.SetActive(true);

            thrust = GetComponent<PlayerShooting>().thrust;
			string g = "None";
			if (GetComponent<PlayerShooting> ()._currentGun) {
				print(g = GetComponent<PlayerShooting> ()._currentGun.name);
				g = GetComponent<PlayerShooting> ()._currentGun.name;
			}

            //Quan es clica
			if (Input.GetKeyDown(KeyCode.Mouse0) 
				&& !g.Equals("Dynamite Base")
				&& !g.Equals("Laser Saber")
				&& !g.Equals("None"))
            {
                time += Time.deltaTime;
                UI_bar.rectTransform.sizeDelta = new Vector2(UI_bar.rectTransform.sizeDelta.x + time * barVelocity, UI_bar.rectTransform.sizeDelta.y);
            }

            //Mentre esta apretat, incrementem
			if (Input.GetKey(KeyCode.Mouse0) 
				&& !g.Equals("Dynamite Base") 
				&& !g.Equals("Laser Saber")
				&& !g.Equals("None"))
            {
                if (time <= maxShootTime)
                {
                    time += Time.deltaTime;
                    UI_bar.rectTransform.sizeDelta = new Vector2(UI_bar.rectTransform.sizeDelta.x + time * barVelocity, UI_bar.rectTransform.sizeDelta.y);
                }
                    
                //Format del % en text
                float timest = ((time * 100f) / maxShootTime);
                if(timest < 100f)
                    UI_text.text = ((time * 100f) / maxShootTime).ToString("00") + "%";
                if(timest >= 100f)
                    UI_text.text = ((time * 100f) / maxShootTime).ToString("000") + "%";

            }
            else
            {
                UI_text.text = "0 %";
            }

            //Quan es lliura el click, reiniciem
            if (Input.GetKeyUp(KeyCode.Mouse0) )
            {
                time = 0f;

                UI_bar.rectTransform.sizeDelta = new Vector2(1f, UI_bar.rectTransform.sizeDelta.y);
            }
            

            if(time > 0f && time <= maxShootTime*0.2f)
            {
                UI_bar.color = bar_colors[0];
            }
            else if( time > maxShootTime*0.2f && time <= maxShootTime * 0.5f)
            {
                UI_bar.color = bar_colors[1];
            }
            else if (time > maxShootTime * 0.5f && time <= maxShootTime * 0.7f)
            {
                UI_bar.color = bar_colors[2];
            }
            else if (time > maxShootTime * 0.7f && time <= maxShootTime * 1f)
            {
                UI_bar.color = bar_colors[3];
            }

        }
        else if(control.activePlayer == null)
        {
            UI_bar.gameObject.SetActive(false); //Si ningu esta actiu, desactivem la barra
        }


        

    }


}
