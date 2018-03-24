using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
	[SerializeField]
	private Resource resource;
	[SerializeField]
	private float time;
	[SerializeField]
	private float resourcePerTime;

	[SerializeField]
	private float maxResource;

	public Resource GeneratingResource
	{
		get { return resource; }
		set { resource = value; }
	}

	public float Time
	{
		get { return time; }
		set { time = value; }
	}

	public float ResourcePerTime
	{
		get { return resourcePerTime; }
		set { resourcePerTime = value; }
	}

	public float MaxResource
	{
		get { return maxResource; }
	}

	void Start()
	{
		StartCoroutine(MingingCorutine());
	}

	IEnumerator MingingCorutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(time);
			switch (resource)
			{
				case Resource.Iron:
					Inventory.IronResource += ResourcePerTime;
					if (Inventory.IronResource > maxResource)
						Inventory.IronResource = maxResource;
					break;
				case Resource.Oil:
					Inventory.OilResource += ResourcePerTime;
					if (Inventory.OilResource > maxResource)
						Inventory.OilResource = maxResource;
					break;
				default:
					throw new Exception("Unknown resource " + resource);
			}
		}
	}

	public enum Resource
	{
		Iron,
		Oil
	}
}
