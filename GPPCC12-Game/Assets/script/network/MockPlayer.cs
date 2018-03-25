using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MockPlayer : NetworkBehaviour
{


	/// <summary>
	/// The player class of the local player
	/// </summary>
	public static PlayerClass PlayerClass
	{
		get;
		private set;
	}

	/// <summary>
	/// The player class of THIS mock player 
	/// </summary>
	public PlayerClass playerClass;

	void Start()
	{
		if (isLocalPlayer)
		{
			LobbyCanvas.lobbyCanvas.dropdown.onValueChanged.AddListener(arg0 => UpdateSelection());
		}
	}

	/// <summary>
	/// Updates the player class selection
	/// </summary>
	public void UpdateSelection()
	{
		if (isLocalPlayer)
		{
			int sel = LobbyCanvas.lobbyCanvas.dropdown.value;
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
	/// <param name="playerClass">the new player clas</param>
	[Command]
	private void CmdSyncPlayerClass(PlayerClass playerClass)
	{
		this.playerClass = playerClass;
	}

}
