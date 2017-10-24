using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour {

    public bool windActive;
    public bool ignoreMass = false; //bool per determinar si la força del vent s'aplica tenin en conte la massa o no
    [Space(5)]
    public float windForce;
    public Vector2 windDirection;
    //[Space(5)]
    //public string[] tagsToApplyWind = { "Player", "Bullet" };
    [Space(5)]
    public List<GameObject> objectsWind = new List<GameObject>();

    // Use this for initialization
    void Start () {
        /* foreach (string tag in tagsToApplyWind){objectsWind.AddRange(GameObject.FindGameObjectsWithTag(tag));} */

        objectsWind.AddRange(GameObject.FindGameObjectsWithTag("Player"));

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (windActive)
        {
            //agafem la municio en aquet frame
            objectsWind.AddRange(GameObject.FindGameObjectsWithTag("Bullet"));

            foreach(GameObject obj in objectsWind)
            {
                if (obj) //(parece tonteria, pero no)
                { 
                    Rigidbody rig;
                    rig = obj.GetComponent<Rigidbody>();
                    if (rig) //si existeix rigidbody en aquest objecte (per si acas no existeix, que no peti)
                    {
                        Vector3 force = new Vector3(windDirection.x * windForce, windDirection.y * windForce, 0f);
                        if (ignoreMass)
                            rig.AddForce(force, ForceMode.Force);
                        else
                            rig.AddForce(force, ForceMode.Acceleration);

                        Debug.Log(" > Applying wind force to: " + obj.name);
                    }
                }
            }
        }

	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, windDirection);
    }
}
