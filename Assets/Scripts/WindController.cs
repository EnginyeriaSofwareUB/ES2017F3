using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour {

    public bool windActive;
    public bool ignoreMass = false; //bool per determinar si la força del vent s'aplica tenin en conte la massa o no
    [Space(5)]
    public float windForce; //min 0 max 5;
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
    float bar_startScale;
    float[] bar_sizes = new float[5];
    //public float barPopChangeTime = 0.25f;

    // Use this for initialization
    void Start () {
        /* foreach (string tag in tagsToApplyWind){objectsWind.AddRange(GameObject.FindGameObjectsWithTag(tag));} */

        bar_startScale = windBars[0].localScale.y;

        int i = 0;
        foreach(float scale in bar_sizes)
        {
            bar_sizes[i] = bar_startScale * i;
            i++;
        }

        objectsWind.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        BarEffect();
    }

    void Update()
    {
        //will get better, dont panic
        if(windForce <= 0)
        {
            BarScale(0f);
        }else if(windForce == 1)
        {
            BarScale(1f);
        }else if(windForce == 2)
        {
            BarScale(2f);
        }else if(windForce == 3)
        {
            BarScale(3f);
        }else if(windForce == 4)
        {
            BarScale(4f);
        }
        else
        {
            BarScale(5f);
        }

    }

    void BarScale(float scale)
    {
        foreach(Transform bar in windBars)
        {
            bar.localScale = new Vector3(bar.localScale.x, scale, bar.localScale.z);
        }
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
