using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchProgressBar : MonoBehaviour
{
    GameController control;

    public Slider progressBar;
    public Image team1BarPart;
    public Image team2BarPart;

    [Space(10)]
    [Header("Health values")]

    private float team1MaxHealth;
    private float team2MaxHealth;

    public float team1Health;
    public float team2Health;

    [Space(5)]

    public float relation;
    public float t1Relation;
    public float t2Relation;

    [Header("Player Icons")]

    public Transform t1Icon;
    public Transform t2Icon;
    public SpriteRenderer t1Back;
    public SpriteRenderer t2Back;
    [Space(5)]
    public Transform back1_backup;
    public Transform back2_backup;
    GameObject a;
    GameObject b;

    [Space(5)]

    public GameObject pirate;
    public GameObject viking;
    public GameObject knight;

    // Use this for initialization
    void Start()
    {
        control = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();

        team1MaxHealth = GamePreferences.players_maxlife;
        team2MaxHealth = GamePreferences.players_maxlife;

        a = new GameObject();
        b = new GameObject();
        //Place the players indicators
        switch (GamePreferences.p1_faction)
        {
            case "pirates":
                a = Instantiate(pirate, t1Icon.position, t1Icon.transform.rotation, t1Icon) as GameObject;
                break;
            case "vikings":
                a = Instantiate(viking, t1Icon.position, t1Icon.transform.rotation, t1Icon) as GameObject;
                break;
            case "knights":
                a = Instantiate(knight, t1Icon.position, t1Icon.transform.rotation, t1Icon) as GameObject;
                break;
        }
        switch (GamePreferences.p2_faction)
        {
            case "pirates":
                b = Instantiate(pirate, t2Icon.position, t2Icon.transform.rotation, t2Icon) as GameObject;
                break;
            case "vikings":
                b = Instantiate(viking, t2Icon.position, t2Icon.transform.rotation, t2Icon) as GameObject;
                break;
            case "knights":
                b = Instantiate(knight, t2Icon.position, t2Icon.transform.rotation, t2Icon) as GameObject;
                break;
        }

        //set player color
        a.GetComponent<ColorTeam>().color = 1;
        b.GetComponent<ColorTeam>().color = 2;

        //GetTeamHPs();

        Invoke("GetTeamHPs", 0.3f); //to avoid some bug related with this script being fastest than game controller
    }

    public void SetBackIconsDefaultPosition()
    {
        t1Back.transform.position = back1_backup.transform.position;
        t1Back.transform.localScale = back1_backup.transform.localScale;

        t2Back.transform.position = back2_backup.transform.position;
        t2Back.transform.localScale = back2_backup.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        //GetTeamHPs();

        /* (0-1 way)
        relation = t1Relation / t2Relation;
        t1Relation = team1Health / team1MaxHealth;
        t2Relation = team2Health / team2MaxHealth;

        if (float.IsNaN(relation))
            relation = 0.5f;
        progressBar.value = relation;*/

        
    }

    public void GetTeamHPs()
    {
        team1Health = 0f;
        team2Health = 0f;

        foreach (GameObject pl in control.team1)
        {
            team1Health += pl.GetComponent<PlayerController>().health;
        }
        foreach (GameObject pl in control.team2)
        {
            team2Health += pl.GetComponent<PlayerController>().health;
        }

        //update bar state
        progressBar.maxValue = team1Health + team2Health;
        progressBar.value = team1Health;

        SetHealthColor();
    }


    void SetHealthColor()
    {
        //for team 1
        if (team1Health >= team1MaxHealth * 0.8f)
        {
            t1Back.color = Color.green;
            t1Back.color = Color.green; 
        }
        else if (team1Health < team1MaxHealth * 0.8f && team1Health >= team1MaxHealth * 0.5f)
        {
            t1Back.color = Color.yellow;
            t1Back.color = Color.yellow;
        }
        else if (team1Health < team1MaxHealth * 0.5f && team1Health >= team1MaxHealth * 0.3f)
        {
            t1Back.color = Color.red;
            t1Back.color = Color.red;
        }
        else if(team1Health < team1MaxHealth * 0.3f)
        {
            t1Back.color = Color.magenta;
            t1Back.color = Color.magenta;
        }

        //for team2
        if (team2Health >= team2MaxHealth * 0.8f)
        {
            t2Back.color = Color.green;
            t2Back.color = Color.green;
        }
        else if (team2Health < team2MaxHealth * 0.8f && team2Health >= team2MaxHealth * 0.5f)
        {
            t2Back.color = Color.yellow;
            t2Back.color = Color.yellow;
        }
        else if (team2Health < team2MaxHealth * 0.5f && team2Health >= team2MaxHealth * 0.3f)
        {
            t2Back.color = Color.red;
            t2Back.color = Color.red;
        }
        else if(team2Health < team2MaxHealth * 0.3f)
        {
            t2Back.color = Color.magenta;
            t2Back.color = Color.magenta;
        }

        //setting alpha value, opaque not wanted 
        t1Back.color = new Color(t1Back.color.r, t1Back.color.g, t1Back.color.b, 0.55f);
        t2Back.color = new Color(t2Back.color.r, t2Back.color.g, t2Back.color.b, 0.55f);
    }

    public void SetPlayerIconsActive(bool active)
    {
        t1Back.gameObject.SetActive(active);
        t2Back.gameObject.SetActive(active);
    }
}