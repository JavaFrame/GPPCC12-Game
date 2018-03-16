using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : Projectile
{

	// Use this for initialization
	void Start ()
	{
	
		TimeoutEvent += (projectile, weapon) => 
		{
			
		};
	}
	
}
