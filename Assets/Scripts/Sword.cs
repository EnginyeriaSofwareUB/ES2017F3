using System.Runtime.InteropServices;
using UnityEngine;

public class Sword : Gun
{

    public override void Shoot(float thrust, float maxPower)
    {
        anim.SetTrigger("attack");
    }
}