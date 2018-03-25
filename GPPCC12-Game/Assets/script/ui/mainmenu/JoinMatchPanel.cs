using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinMatchPanel : MonoBehaviour
{
	[SerializeField]
	private InputField ipTf, portTf;

	[SerializeField]
	private Button joinButton;

	
	void Update ()
	{
		joinButton.enabled = !ipTf.text.Equals("") && !portTf.text.Equals("");
	}

	public void JoinBtn()
	{
		SceneManager.LoadScene("Lobby");
	}
}
