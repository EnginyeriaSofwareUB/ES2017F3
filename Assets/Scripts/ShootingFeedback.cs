using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingFeedback : MonoBehaviour {
    GameController control;
    public GameObject gunBase;
    public Image bar;
    public Image UI_bar;
    public Text UI_text;
    int last_dir;
    public float time = 0f;
    float maxShootTime;

    public Color[] bar_colors;

	// Use this for initialization
	void Start () {
        control = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();
        maxShootTime = GetComponent<PlayerShooting>().maxPowerSeconds;
        UI_bar = GameObject.FindGameObjectWithTag("ShootUI_bar").GetComponent<Image>();
        UI_text = GameObject.FindGameObjectWithTag("ShootUI_text").GetComponent<Text>();

        last_dir = Mathf.RoundToInt(this.gameObject.transform.localScale.x);
        bar.transform.position = gunBase.transform.position;
        UI_bar.rectTransform.sizeDelta = new Vector2(1f, UI_bar.rectTransform.sizeDelta.y);
    }
	
	// Update is called once per frame
	void Update () {
        if (control.activePlayer.Equals(this.gameObject))
        {
            bar.gameObject.SetActive(true);

            bar.transform.position = gunBase.transform.position;

            float movement = GetComponent<PlayerMovement>().horizontal; //positive = right, negative = left
            
            if (movement > 0)
            {
                last_dir = 1;
            }else if (movement < 0)
            {
                last_dir = -1;  
            }

            bar.transform.localScale = new Vector3(last_dir, bar.transform.localScale.y, bar.transform.localScale.z);
            //bar.transform.eulerAngles = new Vector3(bar.transform.eulerAngles.x, bar.transform.eulerAngles.y, -last_dir * gunBase.transform.eulerAngles.z);
            bar.transform.rotation = gunBase.transform.rotation;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                bar.enabled = true;

                time += Time.deltaTime;
                bar.transform.localScale = new Vector3(last_dir + time, bar.transform.localScale.y, bar.transform.localScale.z);

                UI_bar.rectTransform.sizeDelta = new Vector2(UI_bar.rectTransform.sizeDelta.x + time*2f, UI_bar.rectTransform.sizeDelta.y);
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (time <= maxShootTime)
                {
                    time += Time.deltaTime;
                    UI_bar.rectTransform.sizeDelta = new Vector2(UI_bar.rectTransform.sizeDelta.x + time * 2f, UI_bar.rectTransform.sizeDelta.y);
                }
                    
                if (last_dir < 0)
                    bar.transform.localScale = new Vector3(last_dir - time, bar.transform.localScale.y, bar.transform.localScale.z);
                else
                    bar.transform.localScale = new Vector3(last_dir + time, bar.transform.localScale.y, bar.transform.localScale.z);

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

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                bar.transform.localScale = new Vector3(last_dir, bar.transform.localScale.y, bar.transform.localScale.z);
                time = 0f;

                UI_bar.rectTransform.sizeDelta = new Vector2(1f, UI_bar.rectTransform.sizeDelta.y);
                //UI_bar.color = new Color(UI_bar.color.r, UI_bar.color.g, UI_bar.color.b, 0f); //set invisible
                //bar.gameObject.SetActive(false);
                bar.enabled = false;
                print("shot");
            }
            

            if(time > 0f && time <= maxShootTime*0.2f)
            {
                bar.color = bar_colors[0];
                UI_bar.color = bar_colors[0];
            }
            else if( time > maxShootTime*0.2f && time <= maxShootTime * 0.5f)
            {
                bar.color = bar_colors[1];
                UI_bar.color = bar_colors[1];
            }
            else if (time > maxShootTime * 0.5f && time <= maxShootTime * 0.7f)
            {
                bar.color = bar_colors[2];
                UI_bar.color = bar_colors[2];
            }
            else if (time > maxShootTime * 0.7f && time <= maxShootTime * 1f)
            {
                bar.color = bar_colors[3];
                UI_bar.color = bar_colors[3];
            }

        }
        else
        {
            bar.gameObject.SetActive(false);
        }


        

    }


}
