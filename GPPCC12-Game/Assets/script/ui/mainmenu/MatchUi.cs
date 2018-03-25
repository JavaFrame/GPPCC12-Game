using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MatchUi : MonoBehaviour
{
	

	[SerializeField]
	private Text title, playerCount;

	[SerializeField]
	private Button joinBtn;

	private MatchInfoSnapshot match;

	public MatchInfoSnapshot Match
	{
		get { return match; }
		set { match = value; }
	}

	void Start ()
	{
		title.text = match.name;
		playerCount.text = match.currentSize + "/" + match.maxSize;
	}


	void Update()
	{
		joinBtn.enabled = match.currentSize < match.maxSize;
	}

	public void JoinBtn()
	{
		
	}
}
