using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : Projectile
{
	/// <summary>
	/// the gameobject with the explosion effect on it. It's spawned when the grande is destroyed.
	/// </summary>
	public GameObject explosionEffect;
	/// <summary>
	/// the radius of the granade blast. This doesn't effect the explosion effect itself, only the damage/physics
	/// </summary>
	public float radius = 5f;
	/// <summary>
	/// the force of the explosion. Only effects the phyiscs of the granade
	/// </summary>
	public float explosionForce = 700f;

	// Use this for initialization
	void Start ()
	{
		DestroyEvent += (projectile) => 
		{
			Explode();
		};
	}


	/// <summary>
	/// Lets the granade exlode
	/// </summary>
	void Explode()
	{
		GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
		Destroy(explosion, 3f);

		Collider[] coliders = Physics.OverlapSphere(transform.position, radius);

		foreach (var colider in coliders)
		{
			Hurtable h = colider.GetComponent<Hurtable>();
			Rigidbody rb = colider.GetComponent<Rigidbody>();
			if (h != null)
				h.Damaged(this.damage, this.parentWeapon.parent, this.parentWeapon);
			if(rb != null)
				rb.AddExplosionForce(explosionForce, transform.position, radius);
		}

		Destroy(this.gameObject);
	}
}
