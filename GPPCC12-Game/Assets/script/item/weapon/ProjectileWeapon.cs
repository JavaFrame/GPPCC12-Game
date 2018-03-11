using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileWeapon : Weapon
{
	public Projectile projectil;

	void Start()
	{
		TriggerEvent += OnTriggerEvent;
		projectil.parentWeapon = this;
	}

	private void OnTriggerEvent(Weapon weapon)
	{
		SpawnProjectile();
	}

	
	private void SpawnProjectile()
	{
		GameObject go = Instantiate(projectil.gameObject, transform.position, parent.transform.rotation);
		
	}
}
