using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class handels the spawning of the players
/// </summary>
public class PlayerClassInitManager : NetworkManager
{
	/// <summary>
	/// The player class you chosen.
	/// </summary>
	public PlayerClass playerClass;

	/// <summary>
	/// All class prefabs to spawn them
	/// </summary>
	public GameObject[] players;

	/// <summary>
	/// the current PlayerClass GameObject which was spawned 
	/// </summary>
	public GameObject currentGO;


	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
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
	}


	/// <summary>
	/// The possible player classes
	/// </summary>
	public enum PlayerClass
	{
		Fps,
		Rts
	};
}
