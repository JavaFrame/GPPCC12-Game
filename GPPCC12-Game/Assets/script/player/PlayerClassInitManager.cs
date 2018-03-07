using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerClassInitManager : NetworkManager
{
	public PlayerClass playerClass;
	public GameObject[] players;
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



	public enum PlayerClass
	{
		Fps,
		Rts
	};
}
