using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

	private static float _oilResource;
	private static float _ironResource;

	public static float OilResource
	{
		get { return _oilResource; }
		set { _oilResource = value; }
	}

	public static float IronResource
	{
		get { return _ironResource; }
		set { _ironResource = value; }
	}


	[SerializeField]
	private Text oilText;

	[SerializeField]
	private Text ironText;
	
	void Update ()
	{
		oilText.text = OilResource.ToString();
		ironText.text = IronResource.ToString();
	}
}
