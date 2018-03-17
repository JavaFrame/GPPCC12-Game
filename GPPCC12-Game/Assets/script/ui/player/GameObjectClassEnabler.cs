using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectClassEnabler : MonoBehaviour
{
	[SerializeField]
	private PlayerClass expectedPlayerClass;

	[SerializeField]
	private Behaviour[] toDisable;
	// Use this for initialization
	void Start()
	{

		if (MockPlayer.PlayerClass != expectedPlayerClass)
		{
			foreach (var c in toDisable)
			{
				c.enabled = false;
			}
		}
	}

}
