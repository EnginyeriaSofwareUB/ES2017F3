using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTeam : MonoBehaviour {

    public bool show = false;
    public int color = 0; //0 or 1
    public int TEAM;
    //public Material red_color;
    //public Material blue_color;

    [Space(5)]

    public bool isViking = false;
    public GameObject torso;
    public GameObject head;
    public Material t;
    public Material h;


	// Use this for initialization
	void Start () {
        if(!show)
            TEAM = GetComponent<PlayerController>().TEAM;
        if (isViking)
        {
            t = torso.GetComponent<Renderer>().materials[1];
            h = head.GetComponent<Renderer>().material;
        }
        else
        {
            t = torso.GetComponent<Renderer>().material;
            h = head.GetComponent<Renderer>().material;
        }

        if (!show)
        {
            if (TEAM == 1)
            {
                t.SetColor("_Color", Color.red);
                h.SetColor("_Color", Color.red);
            }
            else if (TEAM == 2)
            {
                t.SetColor("_Color", Color.blue);
                h.SetColor("_Color", Color.blue);
            }
        }
        else
        {
            if (color == 1)
            {
                t.SetColor("_Color", Color.red);
                h.SetColor("_Color", Color.red);
            }
            else if (color == 2)
            {
                t.SetColor("_Color", Color.blue);
                h.SetColor("_Color", Color.blue);
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
