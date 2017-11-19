using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour {

    public bool windActive;
    public bool ignoreMass = false; //bool per determinar si la força del vent s'aplica tenin en conte la massa o no
    [Space(5)]
    public float windForce; //min 0 max 5;
    public Vector2 windDirection;
    public int dir = 0;
    //[Space(5)]
    //public string[] tagsToApplyWind = { "Player", "Bullet" };
    [Space(5)]
    public List<GameObject> objectsWind = new List<GameObject>();
    [Space(5)]

    [Header("UI")]
    public RectTransform[] windBars;
    float bar_startScale;

    [Space(10)]
    public GameObject fan_UI;
    public GameObject arrow_UI;
    Vector3 arrow_scale;
    Vector3 fan_scale;

  


    void Start()
    {
        arrow_scale = arrow_UI.transform.localScale;
        fan_scale = fan_UI.transform.localScale;
        bar_startScale = windBars[0].localScale.y;

        objectsWind.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        if(windDirection.x < 0)
        {
            dir = -1;
            foreach (Transform bar in windBars)
            {
                if (bar.gameObject.activeSelf)
                {
                    bar.localScale = new Vector3(bar_startScale * dir, bar_startScale, bar_startScale);

                }
            }
        }
        else
        {
            dir = 1;
            foreach (Transform bar in windBars)
            {
                if (bar.gameObject.activeSelf)
                {
                    bar.localScale = new Vector3(bar_startScale * dir, bar_startScale, bar_startScale);

                }
            }
        }
    }



    void Update()
    {
        
        if (windDirection.x > 0)
        {
            arrow_UI.SetActive(true);
            //arrow_UI.transform.localScale = arrow_scale;
            //SetBarsDirection(1);

            foreach (Transform bar in windBars)
            {
                bar.localScale = new Vector3(1f, bar_startScale, bar_startScale);//Mathf.Abs(bar_startScale) *
            }
            Canvas.ForceUpdateCanvases();

            fan_UI.transform.localScale = fan_scale;
            fan_UI.transform.Rotate(0f, 0f, -30f * windForce * Time.deltaTime);
        }
        else if (windDirection.x < 0)
        {
            arrow_UI.SetActive(true);
            //arrow_UI.transform.localScale = new Vector3(arrow_scale.x * -1f, arrow_scale.y, arrow_scale.z);
            //SetBarsDirection(-1);
            foreach (Transform bar in windBars)
            {
                bar.localScale = new Vector3(-1f, bar_startScale, bar_startScale);//Mathf.Abs(bar_startScale) *
            }
            Canvas.ForceUpdateCanvases();

            fan_UI.transform.localScale = new Vector3(fan_scale.x * -1f, fan_scale.y, fan_scale.z);
            fan_UI.transform.Rotate(0f, 0f, 30f * windForce * Time.deltaTime);
        }
        else
        {
            arrow_UI.SetActive(false);
        }

        SetNumberBars(Mathf.RoundToInt(windForce));
        GarbageClean();

        /* For testing purposes
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            windDirection = new Vector2(0f, 1f);
            //int lastdir = dir;
            windForce = Random.Range(0f, 4.9f);
            float r = Random.Range(0f, 1.1f);
            if (r > 0.5f)
                dir = 1;
            if (r <= 0.5f)
                dir = -1;

            windDirection = new Vector2(dir, 1f);
            //if (lastdir != dir)
            //print("aaaaaaaaaaaaaaaaaaaaaaaaaaa");
            //SetBarsDirection(dir);
            print("[WIND] Random change dir " + dir + ", force " + windForce);
        }*/
    }

    void GarbageClean()
    {
        foreach (GameObject o in objectsWind)
        {
            if (o == null)
                Destroy(o);
        }
    }


    void SetNumberBars(int n)
    {
        int j = 0;
        foreach( Transform bar in windBars)
        {
            if (j < n)
            {
                bar.gameObject.SetActive(true);
                //print("[WIND] n Bars changed");
            }
            else
            {
                bar.gameObject.SetActive(false);
            }
            j++;
        }

        //SetBarsDirection(dir);
    }

    /*
    void SetBarsDirection(int d)
    {

        //print(d + " <> " + dir);
        if (d != dir)
        {
            foreach (Transform bar in windBars)
            {
                //if (bar.gameObject.activeSelf)
                //{
                print("[WIND] Changed direction to "+d);
                bar.localScale = new Vector3( d, bar_startScale, bar_startScale);//Mathf.Abs(bar_startScale) *

                //}
            }
            dir = d;
        }

    }*/


    // Update is called once per frame
    void FixedUpdate () {
        if (windActive)
        {
            //agafem la municio en aquet frame
            foreach(GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
            {
                if (!objectsWind.Contains(bullet))
                {
                    objectsWind.Add(bullet);
                }
            }

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

                        //Debug.Log(" > Applying wind force to: " + obj.name);
                    }
                }
            }
        }

	}


    public void ChangeWindRandom()
    {
        windDirection = new Vector2(0f, 1f);
        //int lastdir = dir;
        windForce = Random.Range(0f, 4.9f);
        float r = Random.Range(0f, 1.1f);
        if (r > 0.5f)
            dir = 1;
        if (r <= 0.5f)
            dir = -1;
        
        windDirection = new Vector2(dir, 1f);
        //if (lastdir != dir)
            //print("aaaaaaaaaaaaaaaaaaaaaaaaaaa");
        //SetBarsDirection(dir);
        //print("[WIND] Random change dir "+dir+", force "+windForce);
    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, windDirection);
    }
}
