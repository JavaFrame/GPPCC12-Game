using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadLauncher : ProjectileWeapon {
	void Update()
	{
		if (Input.GetButtonDown("Shoot"))
		{
			Use();
		}
	}
}
