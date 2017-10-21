using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework.Constraints;
using UnityEngine;

public class ExplosiveBullet : AbstractBullet
{

	public CapsuleCollider Collider;

	public float ExplosionDuration = 1f;

	private Rigidbody _rigidbody;

	private MeshRenderer _meshRenderer;
	private SphereCollider _sphereCollider;

	private new void Start()
	{
		base.Start();
		Collider.enabled = false;
		_meshRenderer = GetComponent<MeshRenderer>();
		_rigidbody = GetComponent<Rigidbody>();
		_sphereCollider = GetComponent<SphereCollider>();
	}

	protected override void DespawnBullet()
	{
		TriggerExplosion();
	}

	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("Player"))
			TriggerExplosion();
	}
	
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Colliding with: " + other.tag);
		if (other.CompareTag("DestructibleCube"))
			Destroy(other.gameObject);
		if (other.CompareTag("Player"))
		{
			Debug.Log("Doing " + CalculateDamage(other.gameObject) + " damage to player");
			other.GetComponent<PlayerController>().Damage(CalculateDamage(other.gameObject));
		}
	}

	private float CalculateDamage(GameObject other)
	{
		return BulletDamage * ((other.transform.position - this.transform.position).magnitude / Collider.radius);
	}
	
	private void TriggerExplosion()
	{
		Debug.Log("Explosion Position: " + transform.position);
		// Disable activities of the bullet and enable the explosion
		_rigidbody.isKinematic = true;
		_meshRenderer.enabled = false;
		_sphereCollider.enabled = false;
		transform.rotation = Quaternion.Euler(0, 0, 0);
		Collider.enabled = true;
		// Do animation and stuff in the future
		Destroy(gameObject, ExplosionDuration);
	}
}
