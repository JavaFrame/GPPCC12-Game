using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHurtable : Hurtable {

	// Use this for initialization
	void Start ()
	{
		DiedEventHandler += (victim, from, weapon) => Destroy(this.gameObject);
		HittedEventHandler += (damage, from, weapon) =>
			Debug.Log("Hitted by " + from.name + " with weapon " + weapon.name + ", damage:" + damage + ", health: " + life);
	}

}
