using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{
	public static Spawner Instance { get; private set; }

	void Awake()
	{
		if(Instance != null)
			Debug.LogError("Multiple Spanwers in Scene!");
		Instance = this;
	}

	[Command]
	public void CmdSpawn(GameObject go)
	{
		NetworkServer.Spawn(go);
	}
}
