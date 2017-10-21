using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("Player Stats")]
    public int TEAM;
    public int playerId;
    public float maxHealth = 100f;
    public float health;
    [Space(5)]
    //public bool facing_right = false;
    [HideInInspector] public int last_dir = 0;

    [Space(5)]
    [Header("TESTIN")]
    //public bool active = false;

    public IntUnityEvent deathEvent;

    //Components
    Animator animator;

    private void Awake()
    {
        gameControl = GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>();
        //if (mainPlayer)
            //gameControl.activePlayer = this.gameObject;

        health = maxHealth;
        currentState = PlayerState.none;

        
        animator = GetComponentInChildren<Animator>();
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

    private void FixedUpdate()
    {
        CheckOrientation();
    }

    void CheckOrientation()
    {
        float movement = GetComponent<PlayerMovement>().horizontal; //positive = right, negative = left

        if (movement > 0)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
            last_dir = 1;
        }else if (movement < 0)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
            last_dir = -1;
        }
        else
        {
            transform.localScale = new Vector3(last_dir, transform.localScale.y, transform.localScale.z);
        }

        /*if (transform.localScale.x > 0)
        {
            facing_right = true;
            last_dir = 1;
        }
        else
        {
            facing_right = false;
            last_dir = -1;
        }


        //If looking left + moving right or looking right + moving left: invert scale
        if (movement > 0 && !facing_right || movement < 0 && facing_right)
            transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
        */
        //If not moving,
        if(movement == 0)
        {
           // transform.localScale = new Vector3(transform.localScale.x * (last_dir), transform.localScale.y, transform.localScale.z);
        }

    }

    /*Aquesta funcio hauria de cridarse desde el suposat BulletScript, on al activarse OnEnterCollider(), si el target es un player, cridar a player.Damage(dany)  */
    public void Damage(float value)
    {
        animator.SetTrigger("hurt");

        if (health - value > 0f)
            health = health - value;
        else
            Dying();
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
