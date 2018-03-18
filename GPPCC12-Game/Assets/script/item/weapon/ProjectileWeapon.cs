using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileWeapon : Weapon
{
	public Spawner.SpawnerPrefab projectil;
    public Spawner spawner;
	public Camera camera;

	void Start()
	{
		TriggerEvent += OnTriggerEvent;
	}

	private void OnTriggerEvent(Weapon weapon)
	{
		spawner.CmdSpawn((int) projectil, transform.position, camera.transform.rotation);
	}
}
