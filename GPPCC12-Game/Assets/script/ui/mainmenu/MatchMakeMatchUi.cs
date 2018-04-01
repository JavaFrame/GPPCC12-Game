using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MatchMakeMatchUi : MonoBehaviour
{

	private MatchInfoSnapshot matchInfo;

	[SerializeField]
	private Text matchName, playerCount;

	[SerializeField]
	private Button joinBtn;

	public MatchInfoSnapshot MatchInfo
	{
		get { return matchInfo; }
		set { matchInfo = value; }
	}

	void Start ()
	{
		matchName.text = matchInfo.name;
		playerCount.text = matchInfo.currentSize + "/" + matchInfo.maxSize + " Players";
	}
	
	void Update ()
	{
		joinBtn.interactable = matchInfo.currentSize < matchInfo.maxSize;
	}


	public void JoinMatch()
	{
		PlayerClassInitLobbyManager.
			Instance.
			matchMaker
			.JoinMatch(
				matchInfo.networkId, "", ", ", "", 0, 0,
			(success, info, mmInfo) =>
			{
				if (success)
				{
					PlayerClassInitLobbyManager.Instance.StartClient(mmInfo);
				}
				else
				{
					MainMenu.Instance.SetMessageText("Joining the match failed because of: " + info);
				}
			});
		MainMenu.Instance.ShowMessagePanel("Connection to match " + matchInfo.name + "...", () =>
		{
			PlayerClassInitLobbyManager.Instance.StopClient();
			MainMenu.Instance.ShowMatchmakingPanel();
		});
	}
}
