using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{

	public Behaviour[] toDisable;
	public Camera sceneCamera;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer)
		{
			foreach (var monoBehaviour in toDisable)
			{
				if(monoBehaviour != null)
					monoBehaviour.enabled = false;
				else
					Debug.LogWarning("MonoBehaviour is null in gameobject " + gameObject.name);
			}
		}
		else
		{
			sceneCamera = Camera.main;
			
		}

	}
	
}
