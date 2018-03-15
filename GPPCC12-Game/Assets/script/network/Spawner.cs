using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{
	public  GameObject[] prefabs;

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
