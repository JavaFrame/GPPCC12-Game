using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

/// <summary>
/// This class handels the spawning of the players
/// </summary>
public class PlayerClassInitLobbyManager : NetworkLobbyManager
{
	/// <summary>
	/// All class prefabs to spawn them
	/// </summary>
	public GameObject[] players;

	public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
	{

		foreach (var identity in conn.playerControllers)
		{
			GameObject identityGO = identity.gameObject;
			if(identityGO == null) continue;
			MockPlayer mock = identityGO.GetComponent<MockPlayer>();
			if(mock == null) continue;
			mock.UpdateSelection();
			GameObject prefab = GetPrefab(mock.playerClass);
			GameObject player = GameObject.Instantiate(prefab);
			return player;
		}

		Debug.LogWarning("Couldn't find mock player on player with connid " + conn.connectionId);
		return base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
	}


	private GameObject GetPrefab(PlayerClass c)
	{
		int val = (int) c;
		if (val < 0 || val >= players.Length)
		{
			Debug.LogError(String.Format("The PlayerClass {0} (id: {1}) isn't set in the players array", c, val));
			return null;
		}

		return players[val];
	}

} 
