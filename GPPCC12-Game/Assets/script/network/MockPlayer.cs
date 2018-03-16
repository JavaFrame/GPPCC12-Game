using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MockPlayer : NetworkBehaviour
{
	public PlayerClass playerClass;

	void Start()
	{
		if (isLocalPlayer)
		{
			LobbyCanvas.lobbyCanvas.dropdown.onValueChanged.AddListener(arg0 => UpdateSelection());
		}
	}


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
		}
	}

	[Command]
	private void CmdSyncPlayerClass(PlayerClass playerClass)
	{
		this.playerClass = playerClass;
	}

}
