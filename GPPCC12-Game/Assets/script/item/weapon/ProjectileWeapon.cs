using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileWeapon : Weapon
{
	public Projectile projectil;
    public Spawner spawner;

	void Start()
	{
		TriggerEvent += OnTriggerEvent;
		projectil.parentWeapon = this;
	}

	private void OnTriggerEvent(Weapon weapon)
	{
		CmdSpawnProjectile();
	}

	[Command]
	private void CmdSpawnProjectile()
	{
		//GameObject go = Instantiate(projectil.gameObject, spawner.transform.position, spawner.transform.rotation);
        Debug.Log("Projectile " + projectil.gameObject);
        spawner.CmdSpawn(projectil.gameObject, spawner.transform.position, spawner.transform.rotation);
	}
}
