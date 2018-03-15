using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Granade : Projectile
{

	private ParticleSystem particleSystem;
	// Use this for initialization
	void Start ()
	{
		particleSystem = GetComponent<ParticleSystem>();
		TimeoutEvent += (projectile, weapon) => 
		{
			particleSystem.Play();
		};
	}
	
}
