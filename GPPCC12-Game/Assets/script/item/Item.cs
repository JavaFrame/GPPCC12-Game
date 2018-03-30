using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Item : MonoBehaviour
{
	
	/// <summary>
	/// The itemName of the item
	/// </summary>
	[SerializeField]
	private string itemName;

	/// <summary>
	/// the description of the item
	/// </summary>
	[SerializeField]
	private string description;

	/// <summary>
	/// Gets called when the item is used
	/// </summary>
	public abstract bool Use();
}
