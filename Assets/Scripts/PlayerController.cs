using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    GameController gameControl;

    public enum PlayerState //jumping i falling poder son redundants, ens poden ajudar a fer que al caure el vent influeixi en el moviment que fa el jugador.
    {
        none,
        moving,
        jumping,
        falling,
        dying,
    }
    public PlayerState currentState;
    public Transform playerModel;

    [Header("Player Stats")]
    public int TEAM;
    public int playerId;
    public float maxHealth = 100f;
    public float health;
    [Space(5)]
    [HideInInspector] public int last_dir = 0;

    [Space(5)]

    [Header("PlayerCanvas")]
    public bool healthInNumber = false;
    public Slider healthbar;
    public Image healthbar_fill;
    public Text healthtext;
    public Text damageText;
    Canvas playerCanvas;


    public IntUnityEvent deathEvent;

    //Components
    Animator animator;

    private void Awake()
    {
        gameControl = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();
        animator = GetComponentInChildren<Animator>();
        playerCanvas = GetComponentInChildren<Canvas>();

        health = maxHealth;

        InitPlayerCanvas();

        currentState = PlayerState.none;
    }


    void InitPlayerCanvas()
    {
        //init health bar / text
        healthbar.minValue = 0f;
        healthbar.maxValue = maxHealth;
        healthbar.value = health;
        healthtext.text = health.ToString() + "%";

        if (healthInNumber)
            healthbar.gameObject.SetActive(false);
        else
            healthtext.enabled = false;

        SetHealthColor();
    }

    void SetHealthColor()
    {
        if(health >= 80f)
        {
            healthtext.color = Color.green;
            healthbar_fill.color = Color.green;
        }
        else if(health < 80f && health >= 50f)
        {
            healthtext.color = Color.yellow;
            healthbar_fill.color = Color.yellow;
        }
        else if(health < 50f && health >= 30f)
        {
            healthtext.color = Color.red;
            healthbar_fill.color = Color.red;
        }
        else
        {
            healthtext.color = Color.magenta;
            healthbar_fill.color = Color.magenta;
        }
    }


    // Use this for initialization
    void Start () {

        // Init variables

        if (deathEvent == null)
            deathEvent = new IntUnityEvent();

    }
	
	// Update is called once per frame
	void Update () {



        /*animations*/

    }

    /*Aquesta funcio hauria de cridarse desde el suposat BulletScript, on al activarse OnEnterCollider(), si el target es un player, cridar a player.Damage(dany)  */
    public void Damage(float value)
    {
        animator.SetTrigger("hurt");
        damageText.text = "-"+value.ToString();


        if (health - value > 0f)
        {
            health = health - value;

            healthbar.value = health;
            healthtext.text = health.ToString() + "%";

            SetHealthColor();
        }
        else
        {
            Dying();
        }
    }

    void Dying()
    {
        //TO DO: Play death animation & sound
        //TO DO: Obtenir/Comunicarse amb el controlador, d'alla treure quans jugadors per cada equip estan vius, si hi han 0 vius, el gameState del controlador es canvia a GameOver i al pasar el check s'executa la rutina de gameover
        currentState = PlayerState.dying;
        animator.SetTrigger("death");



    }

    public void Death()
    {
        Debug.Log(this.name + " dies.");
        
        // notify all listeners of this death
        if (deathEvent != null) deathEvent.Invoke(playerId);

        //Delete himself from GControl lists
        switch (TEAM)
        {
            case 1:
                gameControl.team1.Remove(this.gameObject);
                break;
            case 2:
                gameControl.team2.Remove(this.gameObject);
                break;
        }

        Destroy(this.gameObject);
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, this.transform.right.normalized);

    }

}
