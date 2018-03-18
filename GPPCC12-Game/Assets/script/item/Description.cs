using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Description : MonoBehaviour
{
	[SerializeField]
	private string _objectTitle;

	[TextArea]
	[SerializeField]
	private string _description;

	public string ObjectTitle
	{
		get { return _objectTitle; }
		set { _objectTitle = value; }
	}

	public string ObjectDescription
	{
		get { return _description; }
		set { _description = value; }
	}
}
