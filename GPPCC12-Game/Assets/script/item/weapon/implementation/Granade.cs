using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : Projectile
{
	public GameObject explosionEffect;
	public float radius = 5f;
	public float explosionForce = 700f;

	// Use this for initialization
	void Start ()
	{
		TimeoutEvent += (projectile, weapon) => 
		{
			Explode();
		};
	}



	void Explode()
	{
		Instantiate(explosionEffect, transform.position, transform.rotation);

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
