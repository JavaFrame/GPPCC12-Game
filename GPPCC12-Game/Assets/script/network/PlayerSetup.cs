using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{

	public MonoBehaviour[] toDisable;
	public Camera sceneCamera;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer)
		{
			foreach (var monoBehaviour in toDisable)
			{
				monoBehaviour.enabled = false;
			}
		}
		else
		{
			sceneCamera = Camera.main;
			
		}

	}
	
}
