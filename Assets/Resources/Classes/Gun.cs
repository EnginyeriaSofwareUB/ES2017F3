using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public Transform SpawnPoint;
    public GameObject BulletPrefab;
    
    public void Shoot(float thrust)
    {
        var bullet = (GameObject)Instantiate(
            BulletPrefab,
            SpawnPoint.position,
            Quaternion.Euler(0, 0, SpawnPoint.rotation.z));

        // Add force to the bullet (vector = bulletPos - gunPos)
        var shootingVector = bullet.transform.position - transform.position;
        shootingVector.z = 0;
        bullet.GetComponent<Rigidbody>().AddForce(shootingVector.normalized * thrust, ForceMode.Impulse);

    }
}
