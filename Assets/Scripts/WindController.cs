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
    [Space(5)]

    [Header("UI")]
    public Transform[] windBars;
    public Transform currentBar ;
    public int currentBarIdx = 0;
    public float barPopDistance = 0.5f;
    public float barPopDuration = 0.25f;
    //public float barPopChangeTime = 0.25f;

    // Use this for initialization
    void Start () {
        /* foreach (string tag in tagsToApplyWind){objectsWind.AddRange(GameObject.FindGameObjectsWithTag(tag));} */

        objectsWind.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        BarEffect();
    }

    void Update()
    {

    }

    void BarEffect()
    {
        if (windDirection.x > 0)
        {
            currentBar = windBars[currentBarIdx];
            StimulateCurrentBar();
            if (currentBarIdx + 1 >= windBars.Length)
                currentBarIdx = 0;
            else
                currentBarIdx++;
        }
        else if(windDirection.x < 0)
        {
            currentBar = windBars[currentBarIdx];
            StimulateCurrentBar();
            if (currentBarIdx - 1 < 0)
                currentBarIdx = windBars.Length-1;
            else
                currentBarIdx--;
        }
        else
        {

        }
        
    }


    void StimulateCurrentBar() //move a lil bit up
    {
        currentBar.position = new Vector3(currentBar.position.x, currentBar.position.y + barPopDistance, currentBar.position.z);
        Invoke("DestimulateCurrentBar", barPopDuration);
    }

    void DestimulateCurrentBar() //move lil bit down
    {
        currentBar.position = new Vector3(currentBar.position.x, currentBar.position.y - barPopDistance, currentBar.position.z);
        Invoke("BarEffect", 0f);
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
