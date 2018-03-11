using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Description : MonoBehaviour
{
	[TextArea]
	[SerializeField]
	private string _description;

	public string ObjectDescription
	{
		get { return _description; }
		set { _description = value; }
	}
}
