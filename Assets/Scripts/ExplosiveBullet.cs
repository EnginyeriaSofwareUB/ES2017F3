using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework.Constraints;
using UnityEngine;

public class ExplosiveBullet : AbstractBullet
{

	public CapsuleCollider ExplosiveArea;

	public float ExplosionDuration = 1f;

	private Rigidbody _rigidbody;

	private MeshRenderer _meshRenderer;
	private MeshCollider _meshCollider;

	private bool _isExploding;

	new void Start()
	{
		base.Start();
		ExplosiveArea.enabled = false;
		_meshRenderer = GetComponent<MeshRenderer>();
		_rigidbody = GetComponent<Rigidbody>();
		_meshCollider = GetComponent<MeshCollider>();
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

	protected float CalculateDamage(GameObject other)
	{
		return BulletDamage * ((other.transform.position - this.transform.position).magnitude / ExplosiveArea.radius);
	}
	
	protected void TriggerExplosion()
	{
		Debug.Log("Explosion Position: " + transform.position);
		// Disable activities of the bullet and enable the explosion
		_rigidbody.isKinematic = true;
		_meshRenderer.enabled = false;
		_meshCollider.enabled = false;
		transform.rotation = Quaternion.Euler(0, 0, 0);
		_isExploding = true;
		ExplosiveArea.enabled = true;
		// Do animation and stuff in the future
		Destroy(gameObject, ExplosionDuration);
	}
}
