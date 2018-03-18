using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiReferrer : MonoBehaviour
{
	public UiParent canvasPrefab;
	public GameObject canvasInstance;

	void Start()
	{
		if(canvasPrefab == null)
			Debug.LogWarning("CanvasPrefab isn't set on GameObject " + gameObject.name);
	}
}
