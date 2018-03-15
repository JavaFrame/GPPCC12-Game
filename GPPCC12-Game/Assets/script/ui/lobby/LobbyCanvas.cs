using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCanvas : MonoBehaviour
{

	public static LobbyCanvas lobbyCanvas;

	public Canvas canvas;

	public Dropdown dropdown;
	// Use this for initialization
	void Start ()
	{
		lobbyCanvas = this;
	}
	
}
