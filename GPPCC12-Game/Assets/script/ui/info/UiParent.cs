using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiParent : MonoBehaviour
{
	[SerializeField]
	private GameObject parent;

	public GameObject Parent
	{
		get { return parent; }
		set { parent = value; }
	}
}
