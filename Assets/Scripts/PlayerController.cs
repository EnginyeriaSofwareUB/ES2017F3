using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
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
    public float maxHealth = 100f;
    public float health;

    private void Awake()
    {
        health = maxHealth;
        currentState = PlayerState.none;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (health <= 0f)
            Death();
	}



    /*Aquesta funcio hauria de cridarse desde el suposat BulletScript, on al activarse OnEnterCollider(), si el target es un player, cridar a player.Damage(dany)  */
    public void Damage(float value)
    {
        if (health - value > 0f)
            health = health - value;
        else
            Death();
    }


    void Death()
    {
        //TO DO: Play death animation & sound
        //TO DO: Obtenir/Comunicarse amb el controlador, d'alla treure quans jugadors per cada equip estan vius, si hi han 0 vius, el gameState del controlador es canvia a GameOver i al pasar el check s'executa la rutina de gameover
        currentState = PlayerState.dying;

        Debug.Log(this.name + " dies.");
        Destroy(this.gameObject); //Quan hi hagi animacions, la mort es cridara desde la funcio InstantDeath, 
                                  //aquesta funcio es per si es vol executar codi quan estigui morint i 
                                  //per no destruir el objecte abans que la animacio de mort acabi
    }
}
