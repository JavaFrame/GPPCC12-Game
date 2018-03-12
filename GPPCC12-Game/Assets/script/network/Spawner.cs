using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{
	[Command]
	public void CmdSpawn(GameObject prefab, Vector3 pos, Quaternion rot)
	{
        Debug.Log("Spawn " + prefab);
        GameObject go = Instantiate(prefab, pos, rot);
		NetworkServer.Spawn(go);
	}
}
