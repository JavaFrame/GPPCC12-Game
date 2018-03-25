using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchEndUi : MonoBehaviour
{
	[SerializeField]
	private Text wonLostText;

	void Start ()
	{
		if (GameStateManager.Instance == null)
			throw new Exception("There is no GameStateManager in the scene!");

		if(MockPlayer.PlayerClass == PlayerClass.Rts && GameStateManager.Instance.WinningParty == PlayerClass.Rts) 
			wonLostText.text = "You won!";
		else if (MockPlayer.PlayerClass != PlayerClass.Rts && GameStateManager.Instance.WinningParty != PlayerClass.Rts)
			wonLostText.text = "Your team won!";
		else
			wonLostText.text = "You lost!";
		Debug.Log("player: " + MockPlayer.PlayerClass + ", winning: " + GameStateManager.Instance.WinningParty);
	}
}
