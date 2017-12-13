using System.Runtime.InteropServices;
using UnityEngine;

public class Sword : Gun
{

    public override void Shoot(float thrust, float maxPower)
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<ChangeSaberColor>().RandomAttackSound());

        anim.SetTrigger("attack");

        //tell controler that we finished attacking
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>().shoot_ongoing = false;
    }
}