using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuArrowSelector : MonoBehaviour {
    public Botones botones;
    public Text nPlayers;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {        

    }

    public void ChangeNPlayers(int value)
    {
        int newval = (int.Parse(nPlayers.text) + value);
        if(newval <= 4 && newval > 0)
        {
            nPlayers.text = newval.ToString();
        }
    }

}
