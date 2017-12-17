using System.Runtime.CompilerServices;
using UnityEngine;

public class ExplosiveBullet : AbstractBullet
{
    WindController wind; //for deleting itself from wind objects list
	protected CapsuleCollider ExplosiveArea;

	public float ExplosionDuration = 1f;

	public GameObject ExplosionEffect;
	protected Rigidbody _rigidbody;

	protected MeshRenderer[] _meshRenderer;
	protected MeshCollider[] _meshCollider;

	protected bool _isExploding;

    void Awake() {
        ExplosiveArea = GetComponent<CapsuleCollider>();
        ExplosiveArea.enabled = false;
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _meshCollider = GetComponentsInChildren<MeshCollider>();
    }

	public new void Start()
	{
        base.Start();
        wind = GameObject.FindGameObjectWithTag("GM").GetComponent<WindController>();
        ExplosiveArea.enabled = false;
    }

	protected override void DespawnBullet()
	{
		TriggerExplosion();
	}

	// Update is called once per frame
	void Update () {
		
	}
	
	protected void OnTriggerEnter(Collider other)
	{
		if (!_isExploding)
			return;
		//Debug.Log("Colliding with: " + other.tag);
		if (other.CompareTag("DestructibleCube"))
			Destroy(other.gameObject);
		if (other.CompareTag("Player"))
		{
			Debug.Log("Doing " + CalculateDamage(other.gameObject) + " damage to player");
			other.GetComponent<PlayerController>().Damage(CalculateDamage(other.gameObject));

			// Pushback the players inside the trigger
			var pushbackDir = (gameObject.transform.position - other.transform.position);
			pushbackDir.z = 0;
			other.GetComponent<Rigidbody>().AddForce(new Vector3(pushbackDir.normalized.x * (-5),Mathf.Abs(pushbackDir.normalized.y * 9),0f) , ForceMode.Impulse);
		}


        

    }

	protected float CalculateDamage(GameObject other) {
		//For calculate damage the radius is going to be slightly greater, to create an effect of "Onda expansiva"
	    var playerBase = other.transform.Find("Animator/Model_Center/Model/Character_Hands").transform.position;
        var playerPos = new Vector3(playerBase.x, playerBase.y);
        var bulletPos = new Vector3(transform.position.x, transform.position.y);
	    var radius = ExplosiveArea.radius * 1.5f  * ExplosiveArea.transform.localScale.x;
	    var modulus = (playerPos - bulletPos).magnitude;

        //print("Explosion Radius " + radius);
        //print("Magnitude "+ (playerPos - bulletPos).magnitude);
        //print("Bullet at: " + bulletPos + " // Player at: " + playerPos);
        //print("Ratio damage: " + ((radius-modulus)/radius) );
	    if (radius < modulus) {
			return Mathf.Floor(BulletDamage * 0.4f);
        }
		return Mathf.Floor(BulletDamage * Mathf.Max(((radius - modulus) / radius), 0.4f));
	}
	
	protected void TriggerExplosion() {
	    if (wind == null) return;

        // play explosion sound
        GetComponent<AudioSource>().Play();

        //delete itself from wind objects
        if (wind.objectsWind.Contains(this.gameObject))
            wind.objectsWind.Remove(this.gameObject);

		Debug.Log("Explosion Position: " + transform.position);
		// Disable activities of the bullet and enable the explosion
		_rigidbody.isKinematic = true;
		foreach (var meshCollider in _meshCollider)
		{
			meshCollider.enabled = false;
		}
		foreach (var meshRenderer in _meshRenderer)
		{
			meshRenderer.enabled = false;
		}
		_isExploding = true;
		ExplosiveArea.enabled = true;
		if (ExplosionEffect != null)
		{
			var go = Instantiate(ExplosionEffect, transform.position, transform.rotation);
			Destroy(go, 1f);
		}
		Destroy(gameObject, ExplosionDuration);
	}

    private void OnDestroy()
    {
        //tell controler that we finished attacking
        //GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>().shoot_ongoing = false;
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameController>().OnEndShoot();
    }
}
