using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MockPlayer : NetworkBehaviour
{
	public PlayerClass playerClass;

	void Start()
	{
		
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
		}
	}
}
