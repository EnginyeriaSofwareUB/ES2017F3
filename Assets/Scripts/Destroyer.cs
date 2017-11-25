using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("[DESTROY] "+collider.gameObject.name + " get lost too far.. ");
        Destroy(collider.gameObject);
    }
}
