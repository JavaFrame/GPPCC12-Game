using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCountdownUi : MonoBehaviour
{
	[SerializeField]
	private Text TimeLeftText;

	
	void Update ()
	{
		var core = Core.Instance;
		TimeLeftText.text = "Game ends in " + ((core.MatchTime - (Time.time - core.StartTime)) / 60).ToString("0.00") + " min";
	}
}
