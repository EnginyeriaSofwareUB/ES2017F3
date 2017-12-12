using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuArrowSelector : MonoBehaviour {
    public Botones botones;

    public Text nToSudden;
    public Text nPlayers;
    public Text health;
    public Text damageMult;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeTSudden(int value)
    {
        if (botones.suddenBool.GetComponent<Toggle>().isOn)
        {
            int newval = (int.Parse(damageMult.text) + value);

            //if (newval > 0)
            nToSudden.text = (int.Parse(nToSudden.text) + value).ToString();
        }
    }

    public void ChangeNPlayers(int value)
    {
        int newval = (int.Parse(nPlayers.text) + value);
        if(newval <= 6 && newval > 0)
        {
            nPlayers.text = newval.ToString();
        }
    }

    public void ChangeHealth(int value)
    {
        int newval = (int.Parse(damageMult.text) + value);

        if (newval > 0)
            health.text = (int.Parse(health.text) + value).ToString();
    }

    public void ChangeDamage(int value)
    {
        int newval = (int.Parse(damageMult.text) + value);

        if(newval > 0)
            damageMult.text = newval.ToString();
    }

}
