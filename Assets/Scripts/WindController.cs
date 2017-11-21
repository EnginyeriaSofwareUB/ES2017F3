using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindController : MonoBehaviour {
    WeatherController weather;

    public bool windActive;
    public bool ignoreMass = false; //bool per determinar si la força del vent s'aplica tenin en conte la massa o no
    public bool affectPlayers = true; //bool per determinar si el vent afecta als jugadors o no
    [Space(5)]
    public float windForce; //min 0 max 6;
    public Vector2 windDirection;
    public int direction = 1;
    //[Space(5)]
    //public string[] tagsToApplyWind = { "Player", "Bullet" };
    [Space(5)]
    public List<GameObject> objectsWind = new List<GameObject>();

    [Space(10)]
    [Header("UI")]
    public GameObject fan_UI;
    Vector3 fan_scale;
    public Text windForceText;

    bool firsttime = true;


    void Start()
    {
        weather = GetComponent<WeatherController>();

        fan_scale = fan_UI.transform.localScale;

        if(affectPlayers)   
            objectsWind.AddRange(GameObject.FindGameObjectsWithTag("Player"));

    }



    void Update()
    {
        
        if (windDirection.x > 0)
        {
            direction = 1;

            fan_UI.transform.localScale = fan_scale;
            fan_UI.transform.Rotate(0f, 0f, -40f * windForce * Time.deltaTime);
        }
        else if (windDirection.x < 0)
        {
            direction = -1;

            fan_UI.transform.localScale = new Vector3(fan_scale.x * -1f, fan_scale.y, fan_scale.z);
            fan_UI.transform.Rotate(0f, 0f, 40f * windForce * Time.deltaTime);
        }

        weather.cloudVelocity = windForce * weather.windForceMultiplier;
        Debug.Log("[WIND] Setting clouds speed to: " + windForce + "*"+ weather.windForceMultiplier);
        windForceText.text = windForce.ToString("0.0");

        GarbageClean();

        Canvas.ForceUpdateCanvases();
    }

    void GarbageClean()
    {
        foreach (GameObject o in objectsWind)
        {
            if (o == null)
                Destroy(o);
        }
    }

    /*
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

                        Debug.Log("[WIND] Applying wind force to: " + obj.name);
                    }
                }
            }
        }

	}

    //called onec per turn change
    public void ChangeWindRandom()
    {
        windDirection = new Vector2(0f, 1f);
        //int lastdir = dir;
        windForce = Random.Range(0.7f, 6.1f);

        float dir = 1;
        float r = Random.Range(0f, 1.1f);
        if (r > 0.5f)
            dir = 1;
        if (r <= 0.5f)
            dir = -1;
        

        if (firsttime) //ugly fix
        {
            dir = 1;
            windDirection = new Vector2(dir, 1f);
            direction = Mathf.RoundToInt(dir);
            firsttime = false;
        }
        else
        {
            windDirection = new Vector2(dir, 1f);
            direction = Mathf.RoundToInt(dir);
        }

        //tell the actual clouds to die
        if(GetComponent<WeatherController>().clouds.Count > 0)
        {
            foreach (GameObject cloud in GetComponent<WeatherController>().clouds)
            {
                Debug.Log(cloud);
                //cloud.GetComponent<CloudController>().fade = true; //this is to trigger fade away animation on clouds (destroys them on end)

                if(cloud.GetComponent<CloudController>().dir != dir)
                {
                    cloud.GetComponent<CloudController>().dir = dir;
                }
            }
        }
        
    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, windDirection);
    }
}
