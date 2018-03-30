using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileWeapon : Weapon
{
	public Spawner.SpawnerPrefab projectil;
    //public Spawner spawner;
	public Transform camera;

	private Rigidbody playerRigidbody;

	void Start()
	{
		TriggerEvent += OnTriggerEvent;
		playerRigidbody = parent.GetComponent<Rigidbody>();
	}

	private void OnTriggerEvent(Weapon weapon)
	{
		Spawner.SpawnerInstance.CmdSpawn((int) projectil, transform.position, camera.rotation, playerRigidbody.velocity);
	}
}
