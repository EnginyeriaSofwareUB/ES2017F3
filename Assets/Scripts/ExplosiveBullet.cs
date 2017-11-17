using System.Runtime.CompilerServices;
using UnityEngine;

public class ExplosiveBullet : AbstractBullet
{

	private CapsuleCollider ExplosiveArea;

	public float ExplosionDuration = 1f;

	public GameObject ExplosionEffect;
	private Rigidbody _rigidbody;

	private MeshRenderer[] _meshRenderer;
	private MeshCollider[] _meshCollider;

	private bool _isExploding;

    void Awake() {
        ExplosiveArea = GetComponent<CapsuleCollider>();
        ExplosiveArea.enabled = false;
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _meshCollider = GetComponentsInChildren<MeshCollider>();
    }

	new void Start()
	{
		base.Start();
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
		Debug.Log("Colliding with: " + other.tag);
		if (other.CompareTag("DestructibleCube"))
			Destroy(other.gameObject);
		if (other.CompareTag("Player"))
		{
			Debug.Log("Doing " + CalculateDamage(other.gameObject) + " damage to player");
			other.GetComponent<PlayerController>().Damage(CalculateDamage(other.gameObject));
		}
    }

	protected float CalculateDamage(GameObject other) {
	    var playerBase = other.transform.Find("Animator/Model").transform.position;
        var playerPos = new Vector3(playerBase.x, playerBase.y);
        var bulletPos = new Vector3(transform.position.x, transform.position.y);
	    var radius = ExplosiveArea.radius * ExplosiveArea.transform.localScale.x;
	    var modulus = (playerPos - bulletPos).magnitude;

        //print("Explosion Radius " + radius);
        //print("Magnitude "+ (playerPos - bulletPos).magnitude);
        //print("Bullet at: " + bulletPos + " // Player at: " + playerPos);
        //print("Ratio damage: " + ((radius-modulus)/radius) );
	    if (radius < modulus) {
            return BulletDamage * 0.075f;
        }
		return BulletDamage * Mathf.Max(((radius - modulus) / radius), 0.075f);
	}
	
	protected void TriggerExplosion()
	{
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
		var go = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
		Destroy(go, 1f);
		Destroy(gameObject, ExplosionDuration);
	}
}
