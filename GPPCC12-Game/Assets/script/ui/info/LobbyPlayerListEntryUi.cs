using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerListEntryUi : MonoBehaviour
{
	[SerializeField]
	private Text playerName, playerClass;

	[SerializeField]
	private Toggle readyToggle;

	private MockPlayer player;

	public MockPlayer Player
	{
		get { return player; }
		set
		{
			player = value;
			playerName.text = player.PlayerName;
			player.PlayerReadyEvent += (ready) => readyToggle.isOn = ready;
		}
	}

	void Start ()
	{
		readyToggle.isOn = false;
		player.PlayerNameChangedEvent += name => playerName.text = name;
	}
	
	void Update ()
	{
		playerClass.text = player.playerClass.ToString();
	}
}
