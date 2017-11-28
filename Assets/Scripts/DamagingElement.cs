using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingElement : MonoBehaviour
{

	public float Damage;
	// Use this for initialization
	void Start ()
	{
		
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Collided with " + other);
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerController>().Damage(Damage);
		}
		else if (other.CompareTag("DestructibleCube"))
		{
			Destroy(other.gameObject);
		}
	}
}
