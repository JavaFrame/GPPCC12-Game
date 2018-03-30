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

	[SerializeField]
	private int alivePlayer = 0;


	void Awake()
	{
		if(Instance != null)
			 Destroy(Instance);
		Instance = this;
	}

	private bool matchEnded = false;

	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}

	public void PlayerSpawned()
	{
		alivePlayer++;
	}

	public void PlayerDied()
	{
		alivePlayer--;
		if (alivePlayer <= 0)
		{
			RtsPlayerWon();
		}
	}

	public void RtsPlayerWon()
	{
		matchEnded = true;
		winningParty = PlayerClass.Rts;
		PlayerClassInitLobbyManager.Instance.ChangeScene(matchEndScene);
	}

	public void FpsPlayersWon()
	{
		matchEnded = true;
		winningParty = PlayerClass.Fps;
		PlayerClassInitLobbyManager.Instance.ChangeScene(matchEndScene);
	}
}
