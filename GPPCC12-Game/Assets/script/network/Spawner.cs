using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

public class Spawner : NetworkBehaviour
{
	private static Spawner _spawnerInstance;

	public static Spawner SpawnerInstance
	{
		get { return _spawnerInstance; }
		private set { _spawnerInstance = value; }
	}

	[SerializeField]
	private Vector3[] unitSpawnTransforms;
	[SerializeField]
	private Vector3 unitSpawnPosDiff = new Vector3(3, 3, 3);

	public  GameObject[] prefabs;

	void Start()
	{
		if (isLocalPlayer)
			SpawnerInstance = this;

		unitSpawnTransforms = (from go in GameObject.FindGameObjectsWithTag("UnitSpawn") select go.transform.position).ToArray();
		if(unitSpawnTransforms.Length == 0)
			throw new Exception("No unit spawn points where taged by UnitSpawn");
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

	        var rand = new Random();
	        int tries = 10;
	        Vector3 tempSpawnPos;
	        do {
		        Vector3 originUnitSpawn = unitSpawnTransforms[rand.Next(unitSpawnTransforms.Length-1)];

		        tempSpawnPos = originUnitSpawn + new Vector3((float) (rand.NextDouble() * unitSpawnPosDiff.x),
			                       (float) (rand.NextDouble() * unitSpawnPosDiff.y),
			                       (float) (rand.NextDouble() * unitSpawnPosDiff.z));
		        tries--;
	        }
	        while (Physics.CheckBox(tempSpawnPos, new Vector3(2, 2, 2)) && tries > 0) ;

			GameObject go = Instantiate(prefab, tempSpawnPos, Quaternion.identity);
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
		LauncherBullet, HealingUnit, TankUnit, DpsUnit, LaserBullet
    }
}
