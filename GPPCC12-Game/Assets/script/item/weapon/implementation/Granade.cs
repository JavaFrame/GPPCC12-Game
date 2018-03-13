using System.Collections;
using System.Collections.Generic;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Granade : Projectile
{

	private ParticleSystem particleSystem;
	// Use this for initialization
	void Start ()
	{
		particleSystem = GetComponent<ParticleSystem>();
		HitEvent += (victem, attacker, weapon) =>
		{
			Debug.Log("Hitted");
			particleSystem.Play();
		};
	}
	
}
