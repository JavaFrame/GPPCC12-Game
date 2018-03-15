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

		Debug.Log("@ OnLobbyServerCreateGamePlayer");
		foreach (var identity in conn.playerControllers)
		{
			GameObject identityGO = identity.gameObject;
			if(identityGO == null) continue;
			MockPlayer mock = identityGO.GetComponent<MockPlayer>();
			if(mock == null) continue;
			mock.UpdateSelection();
			Debug.Log("Player class: " + mock.playerClass);
			GameObject prefab = GetPrefab(mock.playerClass);
			Debug.Log(prefab.name);
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

	/*public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		int playerClassInt = (int)playerClass;
		if (players.Length > playerClassInt && players[playerClassInt] != null)
		{
			//currentGO = Instantiate(players[playerClassInt], transform);
			GameObject player = Instantiate(players[playerClassInt]);
			NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		}
		else
		{
			Debug.LogError(String.Format("PlayerClass {0} ({1}) isn't defined players array [{2}]!", playerClass, playerClassInt,
				string.Join(", ", Array.ConvertAll(players, i => i.ToString()))));
		}
	}*/




}
