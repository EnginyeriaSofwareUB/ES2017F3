using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: ANIMATION TO ELIMINATE CLOUD
//TODO: DESTROY CLOUD ON CHANGE TURN

public class CloudController : MonoBehaviour {

	private GameObject target;
	private Rigidbody rb;
	private WeatherController wc;

	public float velocity = 12;
	public Vector3 startPosition = new Vector3(-40, 9, 0);
    public float dir;

	public int minZ = -10;
	public int maxZ = 10;
    public int minY = 5;
    public int maxY = 8;
    public int endX = 40;

    public bool fade = false;

	// Awake call
	void Awake () {
		startPosition = new Vector3(startPosition.x, Random.Range(minY, maxY), Random.Range(minZ, maxZ));
	}

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		wc = GameObject.FindGameObjectWithTag("GM").GetComponent<WeatherController>();
	}
	
	// Update is called once per frame
	void Update () {
        if(dir == 1)
		    rb.velocity = new Vector3(velocity, 0f, 0f);
        else
            rb.velocity = new Vector3(-velocity, 0f, 0f);

        if (fade)
        {
            GetComponent<Animator>().SetTrigger("fade");
            Color c = GetComponent<MeshRenderer>().material.color;
            GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, c.a - Time.deltaTime);

            if (c.a - Time.deltaTime <= 0f)
                Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if(wc.clouds.Contains(this.gameObject))
            wc.clouds.Remove(this.gameObject);
    }

}
