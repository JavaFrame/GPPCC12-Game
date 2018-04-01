
using System;
using UnityEngine;
using UnityEngine.Networking;

public class MockPlayer : NetworkLobbyPlayer
{
	/// <summary>
	/// The player class of the local player
	/// </summary>
	public static PlayerClass PlayerClass
	{
		get;
		private set;
	}


	public static MockPlayer LocalMockPlayer
	{
		get;
		private set;
	}

	/// <summary>
	/// The player class of THIS mock player 
	/// </summary>
	public PlayerClass playerClass;

	[SerializeField]
	private string playerName;

	public string PlayerName
	{
		get { return playerName; }
		set
		{
			playerName = value;
			if(isLocalPlayer)
				CmdSyncPlayerName(playerName);
			if(PlayerNameChangedEvent != null)
				PlayerNameChangedEvent.Invoke(playerName);
		}
	}

	public event Action<bool> PlayerReadyEvent;
	public event Action<string> PlayerNameChangedEvent;

	private bool lastReady;

	void Awake()
	{
		string[] playerNames = new[]
		{
			"Franz", "Fritz", "Gustav", "Donald", "Dagobert", "Heinz", "Pauline", "Nils", "Charlotte", "Oskar", "Emil",
			"Emma", "Frida", "Matilda", "Clara", "Elisabeth", "Johanna", "Johannes", "Elias", "David", "Eleni", "Korinna",
			"Silvana", "Julia", "Viktoriea", "Sofie", "Maximilian", "Konstantin"
		};
		var rand = new System.Random();
		PlayerName = playerNames[rand.Next(playerNames.Length)];
	}

	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		if (isLocalPlayer)
		{
			MainMenu.Instance.LobbyClassDropdown.onValueChanged.AddListener(arg0 => UpdateSelection());
			LocalMockPlayer = this;
			MainMenu.Instance.LobbyPlayerName.text = playerName;
		}
	}

	void Update()
	{
		if(lastReady != readyToBegin && PlayerReadyEvent != null)
			PlayerReadyEvent.Invoke(readyToBegin);
		lastReady = readyToBegin;
	}

	/// <summary>
	/// Updates the player class selection
	/// </summary>
	public void UpdateSelection()
	{
		if (isLocalPlayer)
		{
			int sel = MainMenu.Instance.LobbyClassDropdown.value;
			if (sel == 0)
				playerClass = PlayerClass.Fps;
			else if (sel == 1)
				playerClass = PlayerClass.Rts;

			CmdSyncPlayerClass(playerClass);
			Debug.Log("Set local player: " + playerClass);
			MockPlayer.PlayerClass = playerClass;
		}
	}

	/// <summary>
	/// This command sets the given player class to the local player class of the mock player.
	/// Remember its a command. Its send from the client to the server.
	/// </summary>
	/// <param playerName="playerClass">the new player clas</param>
	[Command]
	private void CmdSyncPlayerClass(PlayerClass playerClass)
	{
		this.playerClass = playerClass;
	}


	[Command]
	private void CmdSyncPlayerName(string name)
	{
		this.playerName = name;
	}


	public override void OnClientEnterLobby()
	{
		base.OnClientEnterLobby();
		MainMenu.Instance.AddPlayerToLobbyList(this);

	}

	public override void OnClientExitLobby()
	{
		base.OnClientExitLobby();
		MainMenu.Instance.RemovePlayerFromLobbyList(this);

	}
}
