using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{
	private static Spawner _spawnerInstance;

	public static Spawner SpawnerInstance
	{
		get { return _spawnerInstance; }
		private set { _spawnerInstance = value; }
	}

	public  GameObject[] prefabs;

	void Start()
	{
		if (isLocalPlayer)
			SpawnerInstance = this;
	}


	[Command]
	public void CmdSpawn(int sp, Vector3 pos, Quaternion rot)
	{
		if (sp >= 0 && sp < prefabs.Length)
		{
			GameObject prefab = prefabs[sp];
			GameObject go = Instantiate(prefab, pos, rot);
			NetworkServer.Spawn(go);
		}
		else
		{
			Debug.LogError("SpawnerPrefab " + sp + " doesn't exist!");
			return;
		}
	}

    [Command]
    public void CmdSpawnUnit(int sp)
    {
        if (sp >= 0 && sp < prefabs.Length)
        {
            GameObject prefab = prefabs[sp];
	        if (prefab == null)
	        {
		        throw new Exception("Prefab " + ((SpawnerPrefab) sp) + " was null!");
	        }
            GameObject go = Instantiate(prefab);
            NetworkServer.Spawn(go);
        }
        else
        {
            Debug.LogError("SpawnerPrefab " + sp + " doesn't exist!");
            return;
        }
    }

    public enum SpawnerPrefab
	{
		LauncherBullet, HealingUnit, TankUnit, DpsUnit
    }
}
