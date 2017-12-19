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

    public Color player1 = new Color(1f, 0.4f, 0.4f);
    public Color player2 = new Color(0.4f, 0.7f, 1f);

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
                t.SetColor("_Color", player1);
                h.SetColor("_Color", player1);
            }
            else if (TEAM == 2)
            {
                t.SetColor("_Color", player2);
                h.SetColor("_Color", player2);
            }
        }
        else
        {
            if (color == 1)
            {
                t.SetColor("_Color", player1);
                h.SetColor("_Color", player1);
            }
            else if (color == 2)
            {
                t.SetColor("_Color", player2);
                h.SetColor("_Color", player2);
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
