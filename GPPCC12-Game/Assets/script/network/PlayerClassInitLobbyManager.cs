using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handels the spawning of the players
/// </summary>
public class PlayerClassInitLobbyManager : NetworkLobbyManager
{
	public static string Host;
	public static int Port;

	public static PlayerClassInitLobbyManager Instance
	{
		get;
		private set;
	}

	/// <summary>
	/// All class prefabs to spawn them
	/// </summary>
	public GameObject[] players;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(this.gameObject);
			Debug.Log("Destroyed this PlayerClassInitLobbyManagerObject because one already exists!");
			return;
		}

		Instance = this;

		/*this.matchHost = Host;
		this.matchPort = Port;
		this.StartClient();*/
	}

	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		MainMenu.Instance.gameObject.SetActive(false);
		return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
	}

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
			Transform startPosition = GetStartPosition();
			GameObject player = GameObject.Instantiate(prefab);
			player.transform.position = startPosition.position;
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


	public void ChangeScene(string scene)
	{
		ServerChangeScene(scene);
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		MainMenu.Instance.ShowLobbyPanel();
		base.OnClientConnect(conn);
	}

	public override void OnClientError(NetworkConnection conn, int errorCode)
	{
		MainMenu.Instance.SetMessageText("An error occured (" + errorCode + ")");
		base.OnClientError(conn, errorCode);
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		MainMenu.Instance.ShowMainPanel();
		base.OnClientDisconnect(conn);
	}

	public override void OnLobbyServerPlayersReady()
	{
		///StartCoroutine(ReadyCountdownRoutine());
		base.OnLobbyServerPlayersReady();
	}

	/*IEnumerator ReadyCountdownRoutine()
	{
		yield return new W
	}*/
} 
