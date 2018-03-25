using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {
	public static GameStateManager Instance
	{
		get;
		private set;
	}

	[SerializeField]
	private PlayerClass winningParty = PlayerClass.Rts;

	[SerializeField]
	private string matchEndScene;

	public PlayerClass WinningParty
	{
		get { return winningParty; }
		set { winningParty = value; }
	}


	void Awake()
	{
		if(Instance != null)
			throw new Exception("There are multiple GameStateManagers in the scene!");
		Instance = this;
	}

	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}


	public void RtsPlayerWon()
	{
		winningParty = PlayerClass.Rts;
		PlayerClassInitLobbyManager.Instance.ChangeScene(matchEndScene);
	}

	public void FpsPlayersWon()
	{
		winningParty = PlayerClass.Fps;
		PlayerClassInitLobbyManager.Instance.ChangeScene(matchEndScene);
	}
}
