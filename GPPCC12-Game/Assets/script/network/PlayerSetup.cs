using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{

	/// <summary>
	/// All component which should be disabled if the this component doesn't belong to the local player
	/// </summary>
	public Behaviour[] toDisable;
	/// <summary>
	/// the _camera of the player
	/// </summary>
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
