using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour {
	public static GameObject CoreGO
	{
		get;
		private set;
	}

	void Awake () {
		if(CoreGO != null)
			throw new Exception("There are multiple Cores in the scene");
		CoreGO = this.gameObject;
	}

}
