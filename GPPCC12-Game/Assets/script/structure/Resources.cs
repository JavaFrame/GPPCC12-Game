using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour {
	public static Dictionary<ResourceGenerator.Resource, List<GameObject>> ResourcesDictionary
	{
		get;
		private set;
	}

	[SerializeField]
	private ResourceGenerator.Resource resource;

	[SerializeField]
	private int instance = 0;

	void Awake()
	{
		if(ResourcesDictionary == null)
			ResourcesDictionary = new Dictionary<ResourceGenerator.Resource, List<GameObject>>();

		if (!ResourcesDictionary.ContainsKey(resource))
			ResourcesDictionary[resource] = new List<GameObject>();
		var list = ResourcesDictionary[resource];
		for (int i = 0; i <= instance; i++)
		{
			list.Add(null);
		}
		list[instance] = this.gameObject;
	}

}
