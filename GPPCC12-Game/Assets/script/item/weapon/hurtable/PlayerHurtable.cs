using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtable : DeathHurtable {

	// Use this for initialization
	protected  override void Start ()
	{
		GameStateManager.Instance.PlayerSpawned();
		DiedEventHandler += (victim, from, weapon) =>
			GameStateManager.Instance.PlayerDied();
		base.Start();
	}

}
